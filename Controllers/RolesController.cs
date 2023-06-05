using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.ComponentModel.DataAnnotations;

namespace MagistriMVC.Controllers {
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
	}
}
