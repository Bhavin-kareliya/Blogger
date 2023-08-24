using Blogger.API.Utilities;
using Blogger.Domain.Repositories;
using Blogger.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blogger.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class AccountController : Controller
    {
        private IAccountRepository _accountRepository;
        private readonly TokenUtility _tokenUtility;

        public AccountController(IAccountRepository accountRepository, TokenUtility tokenUtility)
        {
            _accountRepository = accountRepository;
            _tokenUtility = tokenUtility;
        }

        /// <summary>
        /// Check user credentials for login
        /// </summary>
        /// <param name="auth"></param>
        /// <returns>returns JWT token if credetials are valid!</returns>
        [HttpPost]
        public async Task<IActionResult> Login(AuthModel auth)
        {
            int id = await _accountRepository.Login(auth);
            if (id == 0) return Unauthorized("Invalid credentials!");
            UserModel user = await _accountRepository.GetUserByEmail(auth.Email);
            string token = _tokenUtility.GenerateJWT(user);
            return Ok(new AuthenticatedModel { JWT = token, User = user });
        }

    }
}
