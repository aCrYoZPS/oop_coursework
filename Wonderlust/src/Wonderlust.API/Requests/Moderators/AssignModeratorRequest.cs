using System.ComponentModel.DataAnnotations;

namespace Wonderlust.API.Requests.Moderators;

public record AssignModeratorRequest(
    [Required] Guid UserId
);