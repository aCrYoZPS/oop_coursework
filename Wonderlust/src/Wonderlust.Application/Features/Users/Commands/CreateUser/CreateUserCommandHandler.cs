using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Users.Dtos;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler(IUserRepository repository, IMapper mapper)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await repository.GetByEmailAsync(request.Email);

        if (existingUser != null)
        {
            throw new AlreadyExistsException($"User with email {request.Email} already exists");
        }

        var user = mapper.Map<User>(request);
        await repository.AddAsync(user);

        return mapper.Map<UserDto>(user);
    }
}