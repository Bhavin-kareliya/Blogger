using Blogger.Domain.Models;
using Blogger.Domain.ViewModels;

namespace Blogger.Domain.Interfaces;

public interface IPostRepository
{
    /// <summary>
    /// Returns all posts which are published
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Post>> GetAllPost();

    /// <summary>
    /// Returns all posts of specific user including unpublished
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IEnumerable<Post>> GetAllPostByUserId(int userId);

    /// <summary>
    /// Return detailed information about specific post
    /// </summary>
    /// <returns></returns>
    Task<PostDetailModel> GetPostById(int postId);

    /// <summary>
    /// Create new post
    /// </summary>
    /// <param name="post"></param>
    /// <param name="createdBy">User ID</param>
    /// <returns>Inserted PostID</returns>
    Task<int> InsertPost(PostModel post, int createdBy);

    /// <summary>
    /// Update existing post details
    /// </summary>
    /// <param name="id">PostID</param>
    /// <param name="post"></param>
    /// <returns>Affected rows</returns>
    Task<int> UpdatePost(int id, PostModel post);

    /// <summary>
    /// Delete post data
    /// </summary>
    /// <param name="postId">Post ID</param>
    /// <returns>Affected rows</returns>
    Task<int> DeletePost(int postId);
}