using MediatR;

namespace Wonderlust.Application.Features.Moderators.Commands.RevokeModerator;

public record RevokeModeratorCommand(Guid UserId, Guid CommunityId, Guid SenderId) : IRequest;