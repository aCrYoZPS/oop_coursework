using MediatR;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Posts.Commands.DeletePost;

public class DeletePostCommandHandler(IPostRepository postRepository) : IRequestHandler<DeletePostCommand>
{
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        await postRepository.DeleteAsync(request.PostId);
    }
}