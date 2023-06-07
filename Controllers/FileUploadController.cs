using MagistriMVC.Models;
using MagistriMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Xml;

namespace MagistriMVC.Controllers {
	[Authorize(Roles = "Admin,Teacher,Director")]

	public class FileUploadController : Controller {
		StudentsService studentsService;
		public FileUploadController(StudentsService studentsService) {
			this.studentsService = studentsService;
		}
		[HttpPost]
		public async Task<IActionResult> Upload(IFormFile file) { // vysledek z file dialogu (== input name="file")
			string filePath = "";
			if (file.Length > 0) {
				filePath = Path.GetFullPath(file.FileName);
				using (var stream = new FileStream(filePath, FileMode.Create)) {
					await file.CopyToAsync(stream);
					stream.Close();
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.Load(filePath);
					XmlElement koren = xmlDoc.DocumentElement;
					foreach (XmlNode node in koren.SelectNodes("/Students/Student")) {
						Student s = new Student {
							FirstName = node.ChildNodes[0].InnerText,
							LastName = node.ChildNodes[1].InnerText,
							DateOfBirth = DateTime.Parse(node.ChildNodes[2].InnerText, CultureInfo.CreateSpecificCulture("cs-CZ"))
						};
						await studentsService.CreateAsync(s);
					}
				}
				return RedirectToAction("Index", "Students");
			}
			else return View("NotFound");

		}
	}
}
