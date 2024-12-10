namespace FinalProject.Models
{
    public class Comment
    {
        public string Id { get; set; } 
        public string UserId {  get; set; }
        public User? User { get; set; }
        public string CompositionId { get; set; }
        public Composition? Composition { get; set; }
        public string? Text { get; set; }
        public int Rating { get; set; }

        public Comment()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
