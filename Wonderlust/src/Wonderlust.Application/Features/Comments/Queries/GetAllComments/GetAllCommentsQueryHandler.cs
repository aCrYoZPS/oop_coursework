using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Comments.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Comments.Queries.GetAllComments;

public class GetAllCommentsQueryHandler(
    IMapper mapper,
    IPostRepository postRepository,
    ICommentRepository commentRepository
) : IRequestHandler<GetAllCommentsQuery, IEnumerable<CommentDto>>
{
    public async Task<IEnumerable<CommentDto>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
    {
        var existingPost = await postRepository.GetByIdAsync(request.PostId);

        if (existingPost == null)
        {
            throw new NotFoundException($"The post with id {request.PostId}");
        }

        var comments = await commentRepository.GetAllByPostAsync(existingPost.Id);
        return comments.Select(mapper.Map<CommentDto>);
    }
}