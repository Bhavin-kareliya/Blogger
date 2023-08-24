using Blogger.Domain.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blogger.API.Utilities
{
    public class TokenUtility
    {
        private readonly IConfiguration _configuration;

        public TokenUtility(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Create JWT token
        /// </summary>
        /// <param name="data">Users information</param>
        /// <returns></returns>
        public string GenerateJWT(UserModel data)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
            {
                Issuer = _configuration.GetValue<string>("Jwt:Issuer"),
                Audience = _configuration.GetValue<string>("Jwt:Audience"),
                Subject = new ClaimsIdentity(new List<Claim>() {
                                new Claim("Id", data.Id.ToString()),
                                new Claim(ClaimTypes.Email, data.Email),
                                new Claim(ClaimTypes.Name, data.Name)
                            }),
                Expires = DateTime.Now.AddDays(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwt = jwtTokenHandler.WriteToken(token);
            return jwt;
        }
    }
}
