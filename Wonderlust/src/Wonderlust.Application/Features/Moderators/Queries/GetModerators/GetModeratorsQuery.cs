using MediatR;
using Wonderlust.Application.Features.Users.Dtos;

namespace Wonderlust.Application.Features.Moderators.Queries.GetModerators;

public record GetModeratorsQuery(Guid CommunityId) : IRequest<IEnumerable<UserDto>>;