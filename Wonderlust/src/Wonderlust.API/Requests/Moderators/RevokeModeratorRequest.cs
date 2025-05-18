using System.ComponentModel.DataAnnotations;

namespace Wonderlust.API.Requests.Moderators;

public record RevokeModeratorRequest(
    [Required] Guid UserId
);