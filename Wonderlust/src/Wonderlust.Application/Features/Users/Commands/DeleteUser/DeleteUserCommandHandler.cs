using MediatR;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository repository)
    : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await repository.GetByIdAsync(request.UserId);
        if (existingUser == null)
        {
            throw new Exception($"User with id {request.UserId} doesn't exist");
        }

        await repository.DeleteAsync(existingUser.Id);
    }
};
