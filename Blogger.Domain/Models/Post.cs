using System.ComponentModel.DataAnnotations;

namespace Blogger.Domain.Models;

public class Post
{
    public int Id { get; set; } = 0;

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public bool IsPublished { get; set; } = false;
    public DateTime? CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int CreatedBy { get; set; }
}
