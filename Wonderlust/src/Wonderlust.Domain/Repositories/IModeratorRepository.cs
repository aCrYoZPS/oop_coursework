using Wonderlust.Domain.Entities;

namespace Wonderlust.Domain.Repositories;

public interface IModeratorRepository
{
    public Task<IEnumerable<Moderator>> GetAllAsync();
    public Task<IEnumerable<Moderator>> GetByCommunityAsync(Guid communityId);
    public Task<Moderator?> GetByIdAsync(Guid id);
    public Task AddAsync(Moderator moderator);
    public Task UpdateAsync(Moderator moderator);
    public Task DeleteAsync(Guid id);
}