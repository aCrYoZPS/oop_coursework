using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.Services.Posts;

public interface IPostService
{
    Task<IEnumerable<Post>> GetPosts(Guid communityId);
    Task<Post> AddPostAsync(Post post);
    Task<Post> UpdatePostAsync(Post post);
    Task DeletePostAsync(Guid postId);
}