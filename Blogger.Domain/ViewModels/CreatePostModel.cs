using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Blogger.Domain.ViewModels
{
    public class CreatePostModel
    {
        [MaxLength(100)]
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public IFormFile? File { get; set; }
        public bool IsPublished { get; set; } = false;
    }
}
