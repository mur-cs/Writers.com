using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Composition
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MinLength(2,ErrorMessage ="Назва твору має бути більшою за 2 символи.")]
        [StringLength(100,ErrorMessage ="Назва твору має бути меншою за 100 символи.")]
        public string Name { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime PublishDate { get; set; }
        public decimal Rating { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
        [ValidateNever]
        public List<Comment> Comments { get; set; }
        public Composition()
        {
            Id=Guid.NewGuid().ToString();
            PublishDate=DateTime.Now;
        }
    }
}
