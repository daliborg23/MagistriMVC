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
		public RolesController(RoleManager<IdentityRole> rolesManager) {
			this.rolesManager = rolesManager;
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
			var roleToEdit = await rolesManager.FindByIdAsync(id);
			ViewData["Id"] = id;
			ViewData["Something"] = rolesManager.GetType();
			if (roleToEdit == null) {
				return View("NotFound");
			}
			return View(roleToEdit);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, [Bind("Id, Name")] IdentityRole role) {
			await rolesManager.UpdateAsync(role);
			return RedirectToAction("Index");
		}


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