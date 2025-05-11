using System.ComponentModel.DataAnnotations;

namespace Wonderlust.API.Requests.Users;

public record CreateUserRequest(
    [Required] string Username,
    [Required] string Email,
    [Required] string Password
);