using Blogger.Domain.Interfaces;
using Blogger.Domain.Models;
using Blogger.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blogger.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/[controller]/[action]")]
    public class PostController : Controller
    {
        private IPostRepository _blogRepository;

        public PostController(IPostRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        /// <summary>
        /// Returns all published posts
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Post> posts = await _blogRepository.GetAllPost();
            return Ok(posts.OrderBy(x => x.PublishedAt).ThenBy(y => y.CreatedAt));
        }

        /// <summary>
        /// Return single post details by given PostID
        /// </summary>
        /// <param name="id">PostId</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            PostDetailModel post = await _blogRepository.GetPostById(id);
            if (post is null)
                return NotFound("Post not found!");
            return Ok(post);
        }

        /// <summary>
        /// Returns all post of specific user
        /// </summary>
        /// <param name="id">UserId</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByUserId(int id)
        {
            IEnumerable<Post> posts = await _blogRepository.GetAllPostByUserId(id);
            return Ok(posts.OrderByDescending(x => x.CreatedAt));
        }

        /// <summary>
        /// Create new post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(PostModel post)
        {
            var userId = Convert.ToInt32(User.FindFirst("Id")!.Value);
            int id = await _blogRepository.InsertPost(post, userId);
            if (id == 0) return BadRequest("Something went wrong while creating post!");
            return Ok($"Post created with id={id} successfully !");
        }

        /// <summary>
        /// Update existing post details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PostModel post)
        {
            int status = await _blogRepository.UpdatePost(id, post);
            if (status == 0) return BadRequest("Something went wrong while updating post!");
            return Ok("Post updated successfully!");
        }

        /// <summary>
        /// Delete post by PostID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int affectedRows = await _blogRepository.DeletePost(id);
            if (affectedRows == 0) return BadRequest("Something went wrong while deleting post!");
            return Ok("Post deleted successfully!");
        }
    }
}
