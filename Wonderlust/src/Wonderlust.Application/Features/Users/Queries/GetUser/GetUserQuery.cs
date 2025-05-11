using MediatR;
using Wonderlust.Application.Features.Users.Dtos;

namespace Wonderlust.Application.Features.Users.Queries.GetUser;

public record GetUserQuery(Guid UserId) : IRequest<UserDto>;