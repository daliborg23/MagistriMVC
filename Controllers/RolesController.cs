using MagistriMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.ComponentModel.DataAnnotations;

namespace MagistriMVC.Controllers {
	[Authorize(Roles = "Admin,Director")]
	public class RolesController : Controller {
		private RoleManager<IdentityRole> rolesManager;
		private UserManager<AppUser> userManager; // NAVIC
		public RolesController(RoleManager<IdentityRole> rolesManager, UserManager<AppUser> userManager) {
			this.rolesManager = rolesManager;
			this.userManager = userManager;
		}
		public IActionResult Index() {
			return View(rolesManager.Roles);
		}
		public async Task<IActionResult> Create([Required] string name) {
			if (ModelState.IsValid) {
				IdentityResult result = await rolesManager.CreateAsync(new IdentityRole(name));
				if (result.Succeeded) {
					return RedirectToAction("Index");
				}
				else
					Errors(result);
			}
			return View();
		}
		private void Errors(IdentityResult result) {
			foreach (IdentityError error in result.Errors)
				ModelState.AddModelError("", error.Description);
		}

		// edit role
		public async Task<IActionResult> Edit(string id) {
			IdentityRole role = await rolesManager.FindByIdAsync(id);
			List<AppUser> members = new List<AppUser>();
			List<AppUser> nonMembers = new List<AppUser>();
			ViewData["Id"] = id;
			ViewData["Something"] = rolesManager.GetType();
			foreach (AppUser user in userManager.Users) { 
				var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
				list.Add(user);
			}
			return View(new RoleEdit { 
				Role = role,
				Members = members,
				NonMembers = nonMembers
			});
		}
        [HttpPost]
        public async Task<IActionResult> Edit(RoleModification model) {
            IdentityResult result;
            if (ModelState.IsValid) {
                foreach (string userId in model.AddIds ?? new string[] { }) {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null) {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { }) {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null) {
                        result = await userManager.RemoveFromRoleAsync(user,
                        model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
            }
            if (ModelState.IsValid)
                return RedirectToAction("Index");
            else
                return await Edit(model.RoleId);
        }


		//[HttpPost] // funguje i bez
		public async Task<IActionResult> Delete(string id) {
            IdentityRole role = await rolesManager.FindByIdAsync(id);
            if (role != null) {
                IdentityResult result = await rolesManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", rolesManager.Roles);
        }
    }
}