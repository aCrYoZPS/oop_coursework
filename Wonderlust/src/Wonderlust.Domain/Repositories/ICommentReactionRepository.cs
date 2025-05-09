using Wonderlust.Domain.Entities;

namespace Wonderlust.Domain.Repositories;

public interface ICommentReactionRepository
{
    public Task<IEnumerable<CommentReaction>> GetAllAsync();
    public Task<IEnumerable<CommentReaction>> GetAllByCommentAsync(Guid commentId);
    public Task<CommentReaction> GetByIdAsync(Guid id);
    public Task AddAsync(CommentReaction commentReaction);
    public Task UpdateAsync(CommentReaction commentReaction);
    public Task DeleteAsync(Guid id);
}