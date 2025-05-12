using MediatR;

namespace Wonderlust.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(
    Guid UserId
) : IRequest;
