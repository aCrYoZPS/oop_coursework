using Wonderlust.Domain.Entities;

namespace Wonderlust.Domain.Repositories;

public interface ISubscriptionRepository
{
    public Task<IEnumerable<Subscription>> GetAllAsync();
    public Task<IEnumerable<Subscription>> GetAllByUserAsync(Guid userId);
    public Task<IEnumerable<Subscription>> GetAllByCommunityAsync(Guid communityId);
    public Task<Subscription?> GetByIdAsync(Guid id);
    public Task AddAsync(Subscription subscription);
    public Task UpdateAsync(Subscription subscription);
    public Task DeleteAsync(Guid id);
    public Task<Subscription?> GetByUserAndCommunityAsync(Guid userId, Guid communityId);
}