using Wonderlust.Domain.Entities;

namespace Wonderlust.Domain.Repositories;

public interface ICommentNotificationRepository
{
    public Task<IEnumerable<CommentNotification>> GetAllAsync();
    public Task<IEnumerable<CommentNotification>> GetByUserAsync(Guid id);
    public Task<CommentNotification?> GetByIdAsync(Guid id);
    public Task AddAsync(CommentNotification commentNotification);
    public Task UpdateAsync(CommentNotification commentNotification);
    public Task DeleteAsync(Guid id);
}