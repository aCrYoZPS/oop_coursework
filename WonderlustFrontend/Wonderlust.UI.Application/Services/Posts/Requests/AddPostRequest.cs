namespace Wonderlust.UI.Application.Services.Posts.Requests;

public class AddPostRequest(string title, string content)
{
    public string Title { get; set; } = title;
    public string Content { get; set; } = content;
}