namespace Wonderlust.Application.Features.Users.Dtos;

public record UserDto(
    Guid Id,
    string Username,
    string Email,
    DateTimeOffset RegistrationDate,
    int Karma
);