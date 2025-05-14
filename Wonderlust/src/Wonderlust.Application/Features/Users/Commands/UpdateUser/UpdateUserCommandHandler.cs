using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Users.Dtos;
using Wonderlust.Application.Security;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<UpdateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByIdAsync(request.UserId);
        var changed = false;

        if (existingUser == null)
        {
            throw new NotFoundException($"User with id {request.UserId} not found");
        }

        if (request.Username != null)
        {
            existingUser.UpdateUsername(request.Username);
            changed = true;
        }

        if (request.Password != null)
        {
            existingUser.ChangePassword(PasswordManager.HashPassword(request.Password));
            changed = true;
        }

        if (request.Email != null)
        {
            existingUser.UpdateEmail(request.Email);
            changed = true;
        }

        try
        {
            if (changed)
            {
                await userRepository.UpdateAsync(existingUser);
            }

            return mapper.Map<UserDto>(existingUser);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new AlreadyExistsException(
                $"The specified email {request.Email} is already associated with another user.");
        }
    }
}