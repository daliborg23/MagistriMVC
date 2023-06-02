using Microsoft.EntityFrameworkCore;

namespace MagistriMVC.Models {
    public class ApplicationDbContext : DbContext {
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options) {

        }
    }
}
