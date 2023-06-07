using Microsoft.AspNetCore.Identity;
// NEPOUZIVAM ZATIM // NAVIC
namespace MagistriMVC.Models {
	public class RoleEdit {
		public IdentityRole Role { get; set; }
		public IEnumerable<AppUser> Members { get; set; }
		public IEnumerable<AppUser> NonMembers { get; set; }
	}
}
