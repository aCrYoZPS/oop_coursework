using MediatR;

namespace Wonderlust.Application.Features.Subscriptions.Commands.Subscribe;

public record SubscribeCommand(Guid UserId, Guid CommunityId) : IRequest;