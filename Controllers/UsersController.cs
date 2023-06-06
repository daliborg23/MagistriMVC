using MagistriMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagistriMVC.Controllers {
    [Authorize(Roles = "Admin,Director")]
    public class UsersController : Controller {
        private UserManager<AppUser> userManager;
		private RoleManager<IdentityRole> rolesManager;
		private IPasswordHasher<AppUser> passwordHasher;
        private IPasswordValidator<AppUser> passwordValidator;
        public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> rolesManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator) {
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
            this.passwordValidator = passwordValidator;
			this.rolesManager = rolesManager;
		}
        public IActionResult Index() {
            return View(userManager.Users);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserVM user) {
            if (ModelState.IsValid) {
                AppUser appUser = new AppUser {
                    UserName = user.Name,
                    Email = user.Email
                };
                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded) {
                    //await userManager.AddToRoleAsync(appUser, "Admin");
                    return RedirectToAction("Index");
                }
                else {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
        public async Task<IActionResult> Edit(string id) {
            AppUser userToEdit = await userManager.FindByIdAsync(id);
			ViewBag.Roles = new SelectList(rolesManager.Roles);
            ViewBag.AssignedRole = await userManager.GetRolesAsync(userToEdit);
			if (userToEdit == null)
                return View("NotFound");
            else
                return View(userToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password, string role) {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null) {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password)) { 
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded) {
                        user.PasswordHash = passwordHasher.HashPassword(user, password); 
                    }
                    else {
                        //Errors(validPass);
                    }
                }
                else {
                    // mozna zrusim at se pri prazdnem heslu necha stare?
                    //ModelState.AddModelError("", "Password cannot be empty");
                }
				if (!string.IsNullOrEmpty(role)) {
					if (role == "delete") {
						var userRoles = await userManager.GetRolesAsync(user);
						if (userRoles.Count > 0) {
							foreach (var userRole in userRoles) {
								await userManager.RemoveFromRoleAsync(user, userRole);
							}
						}
					}
					else {
						if (!await userManager.IsInRoleAsync(user, role)) {
							await userManager.AddToRoleAsync(user, role);
						}
					}
				}
				if (!string.IsNullOrEmpty(email) && (string.IsNullOrEmpty(password) || validPass.Succeeded)) {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded) {
                        return RedirectToAction("Index");
                    }
                    else
                        Errors(result);
                }
			}
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }
        private void Errors(IdentityResult result) {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        public async Task<IActionResult> Delete(string id) {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null) {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }
    }
}