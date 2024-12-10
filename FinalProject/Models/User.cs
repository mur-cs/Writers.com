using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Розмір ім'я користувача має бути не менше 3 символів.")]
        [StringLength(50, ErrorMessage = "Розмір ім'я користувача має бути не більше 50 символів.")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Введіть коректний тип email.")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage = "Розмір паролю має бути не менше 8 символів")]
		[StringLength(50, ErrorMessage = "Розмір пароля має бути не більше 50 символів.")]
		public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage = "Паролі не співпадают.")]
        public string ConfirmPassword { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Composition> Compositions { get; set; }

        public User()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
