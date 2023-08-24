using Blogger.Domain.Interfaces;
using Blogger.Domain.Models;
using Blogger.Domain.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Blogger.Data.Repositories;

public class PostRepository : IPostRepository
{
    private readonly SqlConnection _connection;

    public PostRepository(IConfiguration configuration)
    {
        _connection = new SqlConnection(configuration.GetConnectionString("BloggerDBConnection"));
    }

    public async Task<IEnumerable<Post>> GetAllPost()
    {
        List<Post> posts = new();
        string query = $"Select * from [dbo].[Posts] where IsPublished = 1";
        SqlCommand cmd = new SqlCommand(query, _connection);

        await _connection.OpenAsync();
        var reader = await cmd.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                Post p = new Post
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString()!,
                    Content = reader["Content"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    PublishedAt = (reader["PublishedAt"] is not DBNull) ? Convert.ToDateTime(reader["PublishedAt"]) : null,
                    IsPublished = Convert.ToBoolean(reader["IsPublished"]),
                    CreatedBy = Convert.ToInt32(reader["CreatedBy"]),
                };
                posts.Add(p);
            }
        }
        await _connection.CloseAsync();

        return posts;
    }

    public async Task<IEnumerable<Post>> GetAllPostByUserId(int userId)
    {
        List<Post> posts = new();
        string query = $"Select * from [dbo].[Posts] where CreatedBy = @createdBy";
        SqlCommand cmd = new SqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@createdBy", userId);

        await _connection.OpenAsync();
        var reader = await cmd.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                Post p = new Post
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString()!,
                    Content = reader["Content"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    PublishedAt = (reader["PublishedAt"] is not DBNull) ? Convert.ToDateTime(reader["PublishedAt"]) : null,
                    IsPublished = Convert.ToBoolean(reader["IsPublished"]),
                    CreatedBy = Convert.ToInt32(reader["CreatedBy"]),
                };
                posts.Add(p);
            }
        }
        await _connection.CloseAsync();

        return posts;
    }

    public async Task<PostDetailModel?> GetPostById(int postId)
    {
        string query = $"Select p.*, u.Name as Author from [dbo].[Posts] p join users u on u.Id = p.CreatedBy where p.Id = @id";
        SqlCommand cmd = new SqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", postId);

        await _connection.OpenAsync();
        var reader = await cmd.ExecuteReaderAsync();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                PostDetailModel post = new PostDetailModel
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString()!,
                    Content = reader["Content"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    PublishedAt = (reader["PublishedAt"] is not DBNull) ? Convert.ToDateTime(reader["PublishedAt"]) : null,
                    IsPublished = Convert.ToBoolean(reader["IsPublished"]),
                    CreatedBy = Convert.ToInt32(reader["CreatedBy"]),
                    Author = reader["Author"].ToString()!
                };
                return post;
            }
        }
        await _connection.CloseAsync();

        return null;
    }

    public async Task<int> InsertPost(PostModel post, int createdBy)
    {
        string query = "usp_InsertPost";
        SqlCommand cmd = new SqlCommand(query, _connection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@pTitle", post.Title);
        cmd.Parameters.AddWithValue("@pContent", post.Content);
        cmd.Parameters.AddWithValue("@pIsPublished", post.IsPublished);
        cmd.Parameters.AddWithValue("@pCreatedBy", createdBy);

        await _connection.OpenAsync();
        int postId = (int)(await cmd.ExecuteScalarAsync())!;
        await _connection.CloseAsync();

        return postId;
    }

    public async Task<int> UpdatePost(int id, PostModel post)
    {
        string query = $"usp_UpdatePost";
        SqlCommand cmd = new SqlCommand(query, _connection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@pId", id);
        cmd.Parameters.AddWithValue("@pTitle", post.Title);
        cmd.Parameters.AddWithValue("@pContent", post.Content);
        cmd.Parameters.AddWithValue("@pIsPublished", post.IsPublished);

        await _connection.OpenAsync();
        int status = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        await _connection.CloseAsync();

        return status;
    }

    public async Task<int> DeletePost(int postId)
    {
        string query = $"DELETE [dbo].[Posts] WHERE Id = @id";
        SqlCommand cmd = new SqlCommand(query, _connection);
        cmd.Parameters.AddWithValue("@id", postId);

        await _connection.OpenAsync();
        int res = await cmd.ExecuteNonQueryAsync();
        await _connection.CloseAsync();

        return res;
    }
}
