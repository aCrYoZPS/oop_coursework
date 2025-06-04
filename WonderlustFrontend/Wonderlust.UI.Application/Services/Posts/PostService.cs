using System.Net.Http.Headers;
using System.Text;
using SerializerLib.Json;
using Wonderlust.UI.Application.Services.Posts.Requests;
using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.Services.Posts;

public class PostService(HttpClient httpClient, SessionManager.SessionManager sessionManager) : IPostService
{
    public async Task<IEnumerable<Post>> GetPosts(Guid communityId)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await httpClient.GetAsync($"{communityId}/posts");
        var responseBody = await response.Content.ReadAsStringAsync();
        var posts = JsonSerializer.Deserialize<List<Post>>(responseBody);
        return posts ?? [];
    }

    public async Task<Post> AddPostAsync(Post post)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new StringContent
        (
            JsonSerializer.Serialize(new AddPostRequest(post.Title, post.Content)),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync($"{post.CommunityId}/posts", content);

        return post;
    }

    public async Task<Post> UpdatePostAsync(Post post)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new StringContent
        (
            JsonSerializer.Serialize(new AddPostRequest(post.Title, post.Content)),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PutAsync($"{post.CommunityId}/posts/{post.Id}", content);
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Post>(responseBody);
    }

    public async Task DeletePostAsync(Guid postId, Guid communityId)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await httpClient.DeleteAsync($"{communityId}/posts/{postId}");
        response.EnsureSuccessStatusCode();
    }
}