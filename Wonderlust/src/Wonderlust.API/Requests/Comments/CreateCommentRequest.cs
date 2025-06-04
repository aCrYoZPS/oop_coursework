using System.ComponentModel.DataAnnotations;

namespace Wonderlust.API.Requests.Comments;

public class CreateCommentRequest(
    [Required] string content,
    Guid? parentCommentId
)
{
    public Guid PostId { get; set; }
    public string Content { get; init; } = content;
    public Guid? ParentCommentId { get; init; } = parentCommentId;
    public Guid AuthorId { get; set; }
}