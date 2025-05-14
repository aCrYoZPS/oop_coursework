using MediatR;
using Wonderlust.Application.Features.Users.Dtos;

namespace Wonderlust.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand()
    : IRequest<UserDto>
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public Guid UserId { get; set; }
}