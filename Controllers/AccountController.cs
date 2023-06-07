using MagistriMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;

namespace MagistriMVC.Controllers {
    [Authorize]
    public class AccountController : Controller {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
		[AllowAnonymous]
		public IActionResult AccessDenied() {
			return View();
		}
		[AllowAnonymous] 
        public IActionResult Login(string? returnUrl) { // tohle musim predelat
            LoginVM loginVM = new LoginVM();
            loginVM.ReturnUrl = returnUrl;
            return View(loginVM);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm]LoginVM login) {
            if (ModelState.IsValid) {
                AppUser appUser = await userManager.FindByNameAsync(login.UserName);
                if (appUser != null) {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(appUser,
                    login.Password, login.Remember, false);
                    // captcha
                    var httpClient = new HttpClient();
			        var secretKey = "6LfdUHYmAAAAAMers_lg0PXVC-OeLh0LpmnKAZXz";
			        var url = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={login.recaptchaResponse}";
			        var response = await httpClient.GetAsync(url);
			        if (!response.IsSuccessStatusCode) {
				        return BadRequest("reCAPTCHA verification failed.");
			        }
			        dynamic jsonResult = await response.Content.ReadAsStringAsync();
			        var recaptcha = JsonConvert.DeserializeObject<RecaptchaResponse>(jsonResult);
			        if (!recaptcha.success) {
				        return BadRequest("reCAPTCHA validation failed.");
			        }

                    if (signInResult.Succeeded) {
                        //return Redirect(login.ReturnUrl ?? "/"); // ??
                        return Redirect(login.ReturnUrl ?? "/"); 
                    }
                }
                ModelState.AddModelError(nameof(login.UserName), "Login Failed: Invalid UserName or password");
            }
			return View("Login",login);
        }
        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
		public async Task<IActionResult> Index() {
			return View();
		}
	}
}
