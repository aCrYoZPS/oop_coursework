namespace Wonderlust.Application.Features.Communities.Dtos;

public record CommunityDto(
    Guid Id,
    string Title,
    string Description,
    DateTimeOffset CreationDate
);