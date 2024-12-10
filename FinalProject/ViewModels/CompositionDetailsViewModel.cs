using FinalProject.Models;

namespace FinalProject.ViewModels
{
    public class CompositionDetailsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Genre Genre { get; set; }
        public DateTime PublishDate { get; set; }
        public decimal Rating { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<Comment> Comments { get; set; }
    }



}
