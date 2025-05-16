using MediatR;
using Wonderlust.Application.Features.Users.Dtos;

namespace Wonderlust.Application.Features.Subscriptions.Queries.GetSubscribers;

public record GetSubscribersQuery(Guid CommunityId) : IRequest<IEnumerable<UserDto>>;