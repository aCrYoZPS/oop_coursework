namespace Wonderlust.UI.Application.Services.Comments.Requests;

public class UpdateCommentRequest(string content)
{
    public string Content { get; set; } = content;
}