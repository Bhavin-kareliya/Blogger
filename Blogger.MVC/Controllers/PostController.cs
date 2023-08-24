using Blogger.Domain.Models;
using Blogger.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Blogger.MVC.Controllers
{
    public class PostController : Controller
    {
        readonly HttpClient _httpClient;
        public PostController(IHttpClientFactory httpClientFactory, IHttpContextAccessor accessor)
        {
            _httpClient = httpClientFactory.CreateClient("api");
            var token = accessor.HttpContext?.User.Claims.FirstOrDefault(e => e.Type == "Token")?.Value;
            // Add JWT token to header if user is logged in
            if (token is not null) _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Retrive post of specific user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> MyPosts()
        {
            int id = Convert.ToInt32(User.FindFirstValue("UserId"));
            List<Post>? posts = await _httpClient.GetFromJsonAsync<List<Post>>($"post/GetByUserId/{id}");
            return View(posts);
        }

        /// <summary>
        /// Detail view of single post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            PostDetailModel? post = await _httpClient.GetFromJsonAsync<PostDetailModel>($"post/GetById/{id}");
            if (post is null) return NotFound("Post not found");
            return View(post);
        }

        /// <summary>
        /// Display form to create new post
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new PostModel());
        }

        /// <summary>
        /// Create new post process
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(PostModel post)
        {
            if (!ModelState.IsValid) return View(post);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("post/create", post);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("MyPosts");
            }
            return View(post);
        }

        /// <summary>
        /// Display form to update existing post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditAsync(int id)
        {
            PostDetailModel? post = await _httpClient.GetFromJsonAsync<PostDetailModel>($"post/GetById/{id}");
            if (post is null) return NotFound();
            PostModel postModel = new()
            {
                Title = post.Title,
                Content = post.Content,
                IsPublished = post.IsPublished
            };
            ViewBag.Id = post.Id;
            return View(postModel);
        }

        /// <summary>
        /// Update existing post
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <param name="post">Updated post model</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, PostModel post)
        {
            if (!ModelState.IsValid) return View(post);
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"post/update/{id}", post);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("MyPosts");
            }
            return View(post);
        }

        /// <summary>
        /// Delete post by id
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var res = await _httpClient.DeleteAsync($"post/delete/{id}");
            return RedirectToAction("MyPosts");
        }

    }
}
