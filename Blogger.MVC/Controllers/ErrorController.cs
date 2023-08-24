using Blogger.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blogger.MVC.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Handle global exceptions
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
