using System.ComponentModel.DataAnnotations;

namespace Wonderlust.API.Requests.Communities;

public record CreateCommunityRequest(
    [Required] string Name,
    string? Description
);