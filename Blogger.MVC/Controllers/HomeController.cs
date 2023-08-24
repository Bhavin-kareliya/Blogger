using Blogger.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.MVC.Controllers
{
    public class HomeController : Controller
    {
        readonly HttpClient _httpClient;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("api");
        }

        /// <summary>
        /// Get all published posts 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> IndexAsync()
        {
            List<Post>? posts = await _httpClient.GetFromJsonAsync<List<Post>>($"post/GetAll");
            return View(posts);
        }
    }
}