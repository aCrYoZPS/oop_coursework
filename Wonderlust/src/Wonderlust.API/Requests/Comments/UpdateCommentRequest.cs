using System.ComponentModel.DataAnnotations;

namespace Wonderlust.API.Requests.Comments;

public class UpdateCommentRequest(string content)
{
    [Required]
    public string Content { get; set; } = content;

    public Guid SenderId { get; set; }
    public Guid CommentId { get; set; }
}