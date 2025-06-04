namespace Wonderlust.UI.Application.Services.Comments.Requests;

public class AddCommentRequest(string content, Guid? parentCommentId)
{
    public string Content { get; set; } = content;
    public Guid? ParentCommentId { get; set; } = parentCommentId;
}