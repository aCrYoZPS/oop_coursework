using System.ComponentModel.DataAnnotations;

namespace Wonderlust.API.Requests.Users;

public record DeleteUserRequest(
    [Required] Guid UserId
);
