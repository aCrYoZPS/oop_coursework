using System.ComponentModel.DataAnnotations;

namespace Wonderlust.API.Requests.Users;

public record GetUserRequest(
    [Required] Guid UserId
);