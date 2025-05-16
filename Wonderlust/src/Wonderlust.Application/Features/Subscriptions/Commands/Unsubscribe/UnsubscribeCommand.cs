using MediatR;

namespace Wonderlust.Application.Features.Subscriptions.Commands.Unsubscribe;


public record UnsubscribeCommand(Guid UserId, Guid CommunityId) : IRequest;
