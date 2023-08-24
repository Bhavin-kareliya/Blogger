using System.ComponentModel.DataAnnotations;

namespace Blogger.Domain.ViewModels
{
    public class AuthModel
    {
        [Required]
        [MaxLength(260)]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Please enter valid Email address.")]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
