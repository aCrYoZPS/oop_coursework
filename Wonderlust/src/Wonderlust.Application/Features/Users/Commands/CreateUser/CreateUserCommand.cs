using MediatR;
using Wonderlust.Application.Features.Users.Dtos;

namespace Wonderlust.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Username,
    string Password,
    string Email
) : IRequest<UserDto>;