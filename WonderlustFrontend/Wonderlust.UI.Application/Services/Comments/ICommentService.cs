using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.Services.Comments;

public interface ICommentService
{
    Task<IEnumerable<Comment>> GetComments(Guid postId);
    Task<Comment> AddCommentAsync(Comment comment);
    Task<Comment> UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(Guid commentId);
}