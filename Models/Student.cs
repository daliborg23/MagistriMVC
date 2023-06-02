using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace MagistriMVC.Models {
    public class Student {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
