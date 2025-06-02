using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.Services.Posts;

public class PostService(HttpClient httpClient) : IPostService
{
    private static readonly List<Post> posts =
    [
        new Post
        {
            Title = "FIRST", Content = "firstcontent", CreationDate = DateTimeOffset.UtcNow,
            LastUpdateDate = DateTimeOffset.UtcNow
        },
        new Post
        {
            Title = "SECOND", Content = "secondcontent", CreationDate = DateTimeOffset.UtcNow,
            LastUpdateDate = DateTimeOffset.UtcNow
        },
        new Post
        {
            Title = "THIRD", Content = "thirdcontent", CreationDate = DateTimeOffset.UtcNow,
            LastUpdateDate = DateTimeOffset.UtcNow
        },
    ];

    public async Task<IEnumerable<Post>> GetPosts(Guid communityId)
    {
        return posts;
    }

    public async Task<Post> AddPostAsync(Post post)
    {
        posts.Insert(0, post);
        return post;
    }

    public async Task<Post> UpdatePostAsync(Post post)
    {
        var existing = posts.FirstOrDefault(p => p.Id == post.Id);
        if (existing != null)
        {
            var index = posts.IndexOf(existing);
            posts[index] = post;
        }

        return post;
    }

    public async Task DeletePostAsync(Guid postId)
    {
        var existing = posts.FirstOrDefault(p => p.Id == postId);
        if (existing != null)
        {
            posts.Remove(existing);
        }
    }
}