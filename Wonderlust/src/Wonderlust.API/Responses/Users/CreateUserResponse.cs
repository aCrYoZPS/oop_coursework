using Wonderlust.Application.Features.Users.Dtos;

namespace Wonderlust.API.Responses.Users;

public record CreateUserResponse(UserDto User, string Token);