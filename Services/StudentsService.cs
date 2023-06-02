using MagistriMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace MagistriMVC.Services {
    public class StudentsService {
        public ApplicationDbContext dbContext;
        public StudentsService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Student>> GetAllAsync() {
            return await dbContext.Students.ToListAsync();
        }
        public async Task CreateAsync(Student newStudent) {
            await dbContext.Students.AddAsync(newStudent);
            await dbContext.SaveChangesAsync();
        }
		public async Task<Student> GetByIdAsync(int id) {
			return await dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
		}
		public async Task<Student> GetByFirstNameAsync(string firstName) {
			return await dbContext.Students.FirstOrDefaultAsync(x => x.FirstName == firstName);
		}
		public async Task<Student> GetByLastNameAsync(string lastName) {
			return await dbContext.Students.FirstOrDefaultAsync(x => x.LastName == lastName);
		}
		public async Task<Student> UpdateAsync(int id, Student updatedStudent) {
			dbContext.Students.Update(updatedStudent);
			await dbContext.SaveChangesAsync();
			return updatedStudent;
		}
		public async Task DeleteAsync(int id) {
			var studentToDelete = await dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
			dbContext.Students.Remove(studentToDelete);
			await dbContext.SaveChangesAsync();
		}
		//public async Task EditAsync()

		//public async Task<IEnumerable<StudentModel>> AddOne(StudentModel student) {
		//    return await dbContext.Students.Add(student);
	}
}