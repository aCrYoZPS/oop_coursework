using System.ComponentModel.DataAnnotations;

namespace Wonderlust.API.Requests.Comments;

public class CreateCommentRequest(
    [Required] string Content,
    Guid? ParentPostId
)
{
    public Guid PostId { get; set; }
    public string Content { get; init; } = Content;
    public Guid? ParentPostId { get; init; } = ParentPostId;
    public Guid AuthorId { get; set; }
}