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
            int id = Convert.ToInt32(User.FindFirstValue("Id"));
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
            return View(new CreatePostModel());
        }

        /// <summary>
        /// Create new post process
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreatePostModel post)
        {
            string? fileName = null;
            if (!ModelState.IsValid) return View(post);

            if (post.File is not null)
            {
                if (post.File.Length > 5000000)
                    return BadRequest("File size must be less than 5Mb");
                else if (post.File.ContentType != "image/jpeg" && post.File.ContentType != "image/jpg" && post.File.ContentType != "image/png" && post.File.ContentType != "video/mp4")
                    return BadRequest("The file format is invalid!. Please select JPEG, JPG, PNG, MP4 files.");

                fileName = $"{Guid.NewGuid()}_{post.File.FileName}";
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), (post.File.ContentType == "video/mp4") ? @"wwwroot\videos" : @"wwwroot\images", fileName);
                using (var stream = System.IO.File.Create(filepath))
                {
                    await post.File!.CopyToAsync(stream);
                }
            }

            Post postModel = new()
            {
                Title = post.Title,
                Content = post.Content,
                FilePath = fileName,
                IsPublished = post.IsPublished,
                CreatedBy = Convert.ToInt32(User.FindFirst("Id")!.Value)
            };

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("post/create", postModel);
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
