using Wonderlust.Domain.Entities;

namespace Wonderlust.Domain.Repositories;

public interface IPostReactionRepository
{
    public Task<IEnumerable<PostReaction>> GetAllAsync();
    public Task<IEnumerable<PostReaction>> GetAllByPostAsync(Guid postId);
    public Task<PostReaction> GetByIdAsync(Guid id);
    public Task AddAsync(PostReaction postReaction);
    public Task UpdateAsync(PostReaction postReaction);
    public Task DeleteAsync(Guid id);
}
