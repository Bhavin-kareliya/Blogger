using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Blogger.Domain.ViewModels;

public class PostModel
{
    [MaxLength(100)]
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public string? FilePath { get; set; }
    public bool IsPublished { get; set; } = false;
}
