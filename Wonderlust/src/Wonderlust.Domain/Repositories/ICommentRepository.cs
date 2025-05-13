using Wonderlust.Domain.Entities;

namespace Wonderlust.Domain.Repositories;

public interface ICommentRepository
{
    public Task<IEnumerable<Comment>> GetAllAsync();
    public Task<IEnumerable<Comment>> GetAllByPostAsync(Guid postId);
    public Task<Comment?> GetByIdAsync(Guid id);
    public Task AddAsync(Comment comment);
    public Task UpdateAsync(Comment comment);
    public Task DeleteAsync(Guid id);
}