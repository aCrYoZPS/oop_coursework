using MediatR;
using Wonderlust.Application.Features.Posts.Dtos;

namespace Wonderlust.Application.Features.Posts.Commands.UpdatePost;

public class UpdatePostCommand(string? title, string? content, Guid? imageId) : IRequest<PostDto>
{
    public Guid SenderId { get; set; }
    public Guid PostId { get; set; }
    public string? Title { get; init; } = title;
    public string? Content { get; init; } = content;
    public Guid? ImageId { get; init; } = imageId;
}