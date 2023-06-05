using MagistriMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MagistriMVC.Services {
	public class GradesService {
		public ApplicationDbContext dbContext;
		public GradesService(ApplicationDbContext dbContext) {
			this.dbContext = dbContext;
		}
		public async Task<IEnumerable<Grade>> GetAllAsync() {
			return await dbContext.Grades.Include(x => x.Student).Include(y => y.Subject).ToListAsync();
		}
		public async Task CreateAsync(GradesViewModel newGrade) {
			var gradeToInsert = new Grade() {
				Student = dbContext.Students.FirstOrDefault(s => s.Id == newGrade.StudentId),
				Subject = dbContext.Subjects.FirstOrDefault(s => s.Id == newGrade.SubjectId),
				What = newGrade.What,
				Mark = newGrade.Mark,
				Date = DateTime.Now
			};
			if (gradeToInsert.Student != null && gradeToInsert.Subject != null) {
				await dbContext.Grades.AddAsync(gradeToInsert);
				await dbContext.SaveChangesAsync();
			}
		}
		//public async Task<GradesDropdownsViewModel> GetNewGradesDropdownsValues() {
		//	var gradesDropdownsData = new GradesDropdownsViewModel() {
		//		Students = await dbContext.Students.OrderBy(x => x.LastName + x.FirstName).ToListAsync(), // + x.FirstName ?
		//		Subjects = await dbContext.Subjects.OrderBy(x => x.Name).ToListAsync()
		//	};
		//	return gradesDropdownsData;
		//}
		public async Task<GradesDropdownsViewModel> GetNewGradesDropdownsValues() {
			var students = await dbContext.Students
				.OrderBy(x => x.LastName)
				.ThenBy(x => x.FirstName)
				.ToListAsync();

			var studentList = students.Select(s => new Student {
				Id = s.Id,
				LastName = $"{s.FirstName} {s.LastName}"
			}).ToList();

			var gradesDropdownsData = new GradesDropdownsViewModel {
				Students = studentList,
				Subjects = await dbContext.Subjects.OrderBy(x => x.Name).ToListAsync()
			};
			return gradesDropdownsData;
		}
		public async Task<Grade> GetByIdAsync(int id) {
			return await dbContext.Grades.Include(x => x.Student).Include(y => y.Subject).FirstOrDefaultAsync(z => z.Id == id);
		}
		public async Task UpdateAsync(int id, GradesViewModel updatedGrade) {
			var dbGrade = await dbContext.Grades.FirstOrDefaultAsync(x => x.Id == updatedGrade.Id);
			if (dbGrade != null) {
				dbGrade.Student = dbContext.Students.FirstOrDefault(x => x.Id == updatedGrade.StudentId);
				dbGrade.Subject = dbContext.Subjects.FirstOrDefault(x => x.Id == updatedGrade.SubjectId);
				dbGrade.What = updatedGrade.What;
				dbGrade.Mark = updatedGrade.Mark;
				dbGrade.Date = updatedGrade.Date;
			}
			dbContext.Update(dbGrade);
			await dbContext.SaveChangesAsync();
		}
		public async Task DeleteAsync(int id) {
			var gradeToDelete = dbContext.Grades.FirstOrDefault(g => g.Id == id);
			dbContext.Grades.Remove(gradeToDelete);
			dbContext.SaveChanges();
		}
	}
}

