using AutoMapper;
using MediatR;
using Wonderlust.Application.Features.Comments.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Comments.Queries.GetComment;

public class GetCommentQueryHandler(IMapper mapper, ICommentRepository commentRepository)
    : IRequestHandler<GetCommentQuery, CommentDto>
{
    public async Task<CommentDto> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(request.CommentId);
        return mapper.Map<CommentDto>(comment);
    }
}