using MagistriMVC.Models;
using MagistriMVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagistriMVC.Controllers {
    public class GradesController : Controller {
        GradesService service;
        public GradesController(GradesService service) {
            this.service = service;
        }
        public async Task<IActionResult> Index() {
            var allGrades = await service.GetAllAsync();
            return View(allGrades);
        }
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
	}
}
