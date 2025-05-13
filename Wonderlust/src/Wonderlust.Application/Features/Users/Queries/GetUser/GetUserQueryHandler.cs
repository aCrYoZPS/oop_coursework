using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Users.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Users.Queries.GetUser;

public class GetUserQueryHandler(IUserRepository repository, IMapper mapper)
    : IRequestHandler<GetUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            throw new NotFoundException($"User with id {request.UserId} not found");
        }

        return mapper.Map<UserDto>(user);
    }
}