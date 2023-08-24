using System.ComponentModel.DataAnnotations;

namespace Blogger.Domain.ViewModels
{
    public class UserModel
    {
        public int Id { get; set; }
        
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(260)]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Please enter valid Email address.")]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
