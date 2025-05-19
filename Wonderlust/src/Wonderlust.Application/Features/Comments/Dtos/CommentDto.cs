namespace Wonderlust.Application.Features.Comments.Dtos;

public record CommentDto(
    Guid Id,
    string Content,
    Guid AuhorId,
    Guid ParrentCommentId,
    DateTimeOffset CreationDate,
    DateTimeOffset LastUpdateDate
);
