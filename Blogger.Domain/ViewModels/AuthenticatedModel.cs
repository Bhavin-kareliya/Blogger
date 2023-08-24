namespace Blogger.Domain.ViewModels
{
    //Used to pass user details and token after successful login
    public class AuthenticatedModel
    {
        public string JWT { get; set; }
        public UserModel User { get; set; }
    }
}
