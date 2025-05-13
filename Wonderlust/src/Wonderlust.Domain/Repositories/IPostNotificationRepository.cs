using Wonderlust.Domain.Entities;

namespace Wonderlust.Domain.Repositories;

public interface IPostNotificationRepository
{
    public Task<IEnumerable<PostNotification>> GetAllAsync();
    public Task<IEnumerable<PostNotification>> GetByUserAsync(Guid userId);
    public Task<PostNotification?> GetByIdAsync(Guid id);
    public Task AddAsync(PostNotification postNotification);
    public Task UpdateAsync(PostNotification postNotification);
    public Task DeleteAsync(Guid id);
}