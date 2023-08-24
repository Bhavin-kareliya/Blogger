using Blogger.Domain.ViewModels;

namespace Blogger.Domain.Repositories;

public interface IAccountRepository
{
    /// <summary>
    /// Validate user credetials for login
    /// </summary>
    /// <param name="auth"></param>
    /// <returns></returns>
    Task<int> Login(AuthModel auth);

    /// <summary>
    /// Retrive user details by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<UserModel> GetUserByEmail(string email);
}