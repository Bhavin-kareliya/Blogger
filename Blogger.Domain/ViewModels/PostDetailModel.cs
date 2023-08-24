namespace Blogger.Domain.ViewModels
{
    public class PostDetailModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public int CreatedBy { get; set; }
        public string Author { get; set; }
    }
}
