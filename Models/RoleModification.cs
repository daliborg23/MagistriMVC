using Microsoft.AspNetCore.Identity;
// NEPOUZIVAM ZATIM // NAVIC
namespace MagistriMVC.Models {
	public class RoleModification {
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string[]? AddIds { get; set; }
        public string[]? DeleteIds { get; set; }

    }
}
