using Blogger.Domain.Repositories;
using Blogger.Domain.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Blogger.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SqlConnection _connection;

        public AccountRepository(IConfiguration configuration)
        {
            _connection = new SqlConnection(configuration.GetConnectionString("BloggerDBConnection"));
        }

        public async Task<int> Login(AuthModel auth)
        {
            string query = "usp_Login";
            SqlCommand cmd = new SqlCommand(query, _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pEmail", auth.Email);
            cmd.Parameters.AddWithValue("@pPassword", auth.Password);

            await _connection.OpenAsync();
            int isValid = (int)(await cmd.ExecuteScalarAsync())!;
            await _connection.CloseAsync();

            return isValid;
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            string query = "select Id, Name, Email, CreatedAt from dbo.Users where Email = @Email";
            SqlCommand cmd = new SqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("@Email", email);

            UserModel user = new();

            await _connection.OpenAsync();
            var reader = await cmd.ExecuteReaderAsync();
            while (reader.Read())
            {
                user = new UserModel
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                };

            }
            await _connection.CloseAsync();
            return user;
        }
    }
}
