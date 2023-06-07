using System.ComponentModel.DataAnnotations;

namespace MagistriMVC.Models {
    public class LoginVM {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
        public bool Remember { get; set; }
		[Required]
		public string recaptchaResponse { get; set; }
	}
}
