using FinalProject.Models;

namespace FinalProject.ViewModels
{
	public class AddCommentViewModel
	{
		public string CompositionId { get; set; }
		public string UserId { get; set; }
		public string Name { get; set; }
		public Genre Genre { get; set; }
		public DateTime PublishDate { get; set; }
		public int Rating { get; set; } 
		public string CommentText { get; set; } 
	}

}
