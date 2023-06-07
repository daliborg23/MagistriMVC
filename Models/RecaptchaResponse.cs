namespace MagistriMVC.Models {
	public class RecaptchaResponse {
		public bool success { get; set; }
		public List<string> errorCodes { get; set; }
}
}
