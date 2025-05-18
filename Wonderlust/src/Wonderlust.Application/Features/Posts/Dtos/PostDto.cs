namespace Wonderlust.Application.Features.Posts.Dtos;

public class PostDto
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string? Content { get; init; }
    public Guid? ImageId { get; init; }
    public Guid AuthorId { get; init; }
    public string AuthorName { get; init; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset LastUpdateDate { get; init; }
}