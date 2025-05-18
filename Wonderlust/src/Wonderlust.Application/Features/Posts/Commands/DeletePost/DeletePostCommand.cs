using MediatR;

namespace Wonderlust.Application.Features.Posts.Commands.DeletePost;

public record DeletePostCommand(Guid PostId) : IRequest;