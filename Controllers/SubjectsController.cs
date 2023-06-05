using MagistriMVC.Models;
using MagistriMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagistriMVC.Controllers {
	[Authorize]
	public class SubjectsController : Controller {
		public SubjectsService service;
		public SubjectsController(SubjectsService service) {
			this.service = service;
		}
		public async Task<IActionResult> IndexAsync() {
			var allSubjects = await service.GetAllAsync();
			return View(allSubjects);
		}
		public IActionResult Create() {
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(Subject newSubject) {
			await service.CreateAsync(newSubject);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Edit(int id) {
			var subjectToEdit = await service.GetByIdAsync(id);
			ViewData["Id"] = id;
			ViewData["Something"] = service.GetType();
			if (subjectToEdit == null) {
				return View("NotFound");
			}
			return View(subjectToEdit);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, [Bind("Id, Name")] Subject subject) {
			await service.UpdateAsync(id, subject);
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Delete(int id) {
			var subjectToDelete = await service.GetByIdAsync(id);
			if (subjectToDelete == null) {
				ViewData["Id"] = id;
				ViewData["Something"] = service.GetType();
				return View("NotFound");
			}
			await service.DeleteAsync(id);
			return RedirectToAction("Index");
		}
	}
}
