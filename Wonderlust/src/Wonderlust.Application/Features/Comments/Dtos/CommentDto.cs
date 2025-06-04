namespace Wonderlust.Application.Features.Comments.Dtos;

public record CommentDto(
    Guid Id,
    string Content,
    Guid AuthorId,
    Guid? ParentCommentId,
    Guid PostId,
    DateTimeOffset CreationDate,
    DateTimeOffset LastUpdateDate
);