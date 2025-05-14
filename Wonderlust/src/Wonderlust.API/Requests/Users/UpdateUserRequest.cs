namespace Wonderlust.API.Requests.Users;

public record UpdateUserRequest(
    string? Username,
    string? Email,
    string? Password
);