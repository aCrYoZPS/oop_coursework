namespace Wonderlust.API.Requests.Posts;

public record UpdatePostRequest(
    string? Title,
    string? Content,
    Guid? ImageId
);