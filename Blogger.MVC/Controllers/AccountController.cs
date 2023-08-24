using Blogger.Domain.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blogger.MVC.Controllers;

public class AccountController : Controller
{
    readonly HttpClient _httpClient;
    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("api");
    }

    /// <summary>
    /// Display login page
    /// </summary>
    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(new AuthModel());
    }


    /// <summary>
    /// Check user credentials
    /// </summary>
    /// <param name="creds"></param>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(AuthModel creds, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            var res = await _httpClient.PostAsJsonAsync("account/login", creds);

            if (!res.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid UserName/Password.");
                return View(creds);
            }

            AuthenticatedModel data = (await res.Content.ReadFromJsonAsync<AuthenticatedModel>())!;
            HttpContext.Response.Cookies.Append("token", data.JWT, new CookieOptions
            {
                Expires = DateTime.Now.AddHours(7),
                Secure = true,
                HttpOnly = false,
            });

            List<Claim> claims = new List<Claim>() {
                                        new Claim(ClaimTypes.Email, data.User.Email),
                                        new Claim(ClaimTypes.Name, data.User.Name),
                                        new Claim("UserId", data.User.Id.ToString()),
                                        new Claim("Token", data.JWT)
                                    };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "AccountCookie");
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync("AccountCookie", principal);
            return RedirectToAction("Index", "Home");
        }
        return View(creds);
    }

    /// <summary>
    /// Remove cookie and logout user
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync("AccountCookie");
        return RedirectToAction("Login", "Account");
    }

    /// <summary>
    /// 404 error handler
    /// </summary>
    /// <returns></returns>
    public IActionResult PageNotFound()
    {
        return View();
    }
}
