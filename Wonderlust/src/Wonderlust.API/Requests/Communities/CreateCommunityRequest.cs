using System.ComponentModel.DataAnnotations;

namespace Wonderlust.API.Requests.Communities;

public record CreateCommunityRequest(
    [Required] string Title,
    string? Description
);
