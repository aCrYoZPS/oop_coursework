using Wonderlust.Domain.Entities;

namespace Wonderlust.Domain.Repositories;

public interface IPostRepository
{
    public Task<IEnumerable<Post>> GetAllAsync();
    public Task<IEnumerable<Post>> GetAllByUserAsync(Guid userId);
    public Task<IEnumerable<Post>> GetAllByCommunityAsync(Guid communityId);
    public Task<Post> GetByIdAsync(Guid id);
    public Task AddAsync(Post post);
    public Task UpdateAsync(Post post);
    public Task DeleteAsync(Guid id);
}