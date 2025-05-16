using MediatR;
using Wonderlust.Application.Features.Communities.Dtos;

namespace Wonderlust.Application.Features.Subscriptions.Queries.GetSubscriptions;

public record GetSubscriptionsQuery(Guid UserId) : IRequest<IEnumerable<CommunityDto>>;