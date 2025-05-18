using MediatR;
using Wonderlust.Application.Features.Posts.Dtos;

namespace Wonderlust.Application.Features.Posts.Commands.CreatePost;

public class CreatePostCommand : IRequest<PostDto>
{
    public string Title { get; set; }
    public string? Content { get; set; }
    public Guid? ImageId { get; set; }
    public Guid CommunityId { get; set; }
    public Guid AuthorId { get; set; }
}