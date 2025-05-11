using AutoMapper;
using MediatR;
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
            throw new Exception($"User with id {request.UserId} does not exist");
        }

        return mapper.Map<UserDto>(user);
    }
}