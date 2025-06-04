namespace Wonderlust.Application.Features.Communities.Dtos;

public record CommunityDto(
    Guid Id,
    string Name,
    string Description,
    DateTimeOffset CreationDate,
    Guid CreatorId
);