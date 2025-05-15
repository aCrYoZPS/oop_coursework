namespace Wonderlust.API.Requests.Communities;

public record UpdateCommunityRequest(
    string? Name,
    string? Description
);