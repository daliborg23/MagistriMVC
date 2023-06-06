using MagistriMVC.Models;
using MagistriMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagistriMVC.Controllers {
    [Authorize(Roles = "Admin,Teacher,Director")]
    public class StudentsController : Controller {
        public StudentsService service;
        public StudentsController(StudentsService service) {
            this.service = service;
        }
        public async Task<IActionResult> IndexAsync() {
            var allStudents = await service.GetAllAsync();
            return View(allStudents);
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Student newStudent) {
            await service.CreateAsync(newStudent);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id) {
            var studentToEdit = await service.GetByIdAsync(id);
			ViewData["Id"] = id;
			ViewData["Something"] = service.GetType();
			if (studentToEdit == null) {
                return View("NotFound");
            }
            return View(studentToEdit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FirstName, LastName, DateOfBirth")] Student student) {
            await service.UpdateAsync(id, student);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id) {
            var studentToDelete = await service.GetByIdAsync(id);
            if (studentToDelete == null) {
				ViewData["Id"] = id;
				ViewData["Something"] = service.GetType();
				return View("NotFound");
            }
            await service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
	}
}
