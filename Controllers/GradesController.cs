using MagistriMVC.Models;
using MagistriMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagistriMVC.Controllers {
	[Authorize(Roles = "Admin,Teacher,Director,Parent,Student")]
	public class GradesController : Controller {
		GradesService service;
		public GradesController(GradesService service) {
			this.service = service;
		}
		public async Task<IActionResult> Index() {
			var allGrades = await service.GetAllAsync();
			return View(allGrades);
		}
        [Authorize(Roles = "Admin,Teacher,Director")]
        public async Task<IActionResult> Create() {
			var gradesDropdownsData = await service.GetNewGradesDropdownsValues();
			ViewBag.Students = new SelectList(gradesDropdownsData.Students, "Id", "LastName");
			ViewBag.Subjects = new SelectList(gradesDropdownsData.Subjects, "Id", "Name");
			//return View(gradesDropdownsData);
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(GradesViewModel newGrade) {
			if (!ModelState.IsValid) {
				var gradesDropdownData = await service.GetNewGradesDropdownsValues();
				ViewBag.Students = new SelectList(gradesDropdownData.Students, "Id", "LastName");
				ViewBag.Subject = new SelectList(gradesDropdownData.Subjects, "Id", "Name");
				return View(newGrade);
			}
			await service.CreateAsync(newGrade);
			return RedirectToAction("Index");
		}
        [Authorize(Roles = "Admin,Teacher,Director")]
        public async Task<IActionResult> Edit(int id) {
			var gradeToEdit = await service.GetByIdAsync(id);
			ViewData["Id"] = id;
			ViewData["Something"] = service.GetType();
			if (gradeToEdit == null) {
				return View("NotFound");
			}
			var response = new GradesViewModel() {
				Id = gradeToEdit.Id,
				StudentId = gradeToEdit.Student.Id,
				SubjectId = gradeToEdit.Subject.Id,
				What = gradeToEdit.What,
				Mark = gradeToEdit.Mark,
				//Date = gradeToEdit.Date
				Date = DateTime.Now
			};
			var gradesDropdownsData = await service.GetNewGradesDropdownsValues();
			ViewBag.Students = new SelectList(gradesDropdownsData.Students, "Id", "LastName");
			ViewBag.Subjects = new SelectList(gradesDropdownsData.Subjects, "Id", "Name");
			return View(response);
		}
		[HttpPost]
		public async Task<IActionResult> Edit(int id, GradesViewModel updatedGrade) {
			await service.UpdateAsync(id, updatedGrade);
			return RedirectToAction("Index");
		}
        [Authorize(Roles = "Admin,Teacher,Director")]
        public async Task<IActionResult> DeleteAsync(int id) {
			await service.DeleteAsync(id);
			return RedirectToAction("Index");
		}
	}
}
