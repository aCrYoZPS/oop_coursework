using MediatR;
using Wonderlust.Application.Features.Posts.Dtos;

namespace Wonderlust.Application.Features.Posts.Commands.CreatePost;

public record CreatePostCommand(string Title, string? Content, Guid? ImageId) : IRequest<PostDto>
{
    public Guid CommunityId { get; set; }
    public Guid AuthorId { get; set; }
}