namespace MagistriMVC.Models {
	public class GradesViewModel {
		public int Id { get; set; }
		public int StudentId { get; set; }
		public int SubjectId { get; set; }
		public string What { get; set; }
		public int Mark { get; set; }
		public DateTime Date { get; set; }
	}
}
