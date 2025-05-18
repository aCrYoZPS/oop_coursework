using MediatR;

namespace Wonderlust.Application.Features.Moderators.Commands.AssignModerator;

public record AssignModeratorCommand(Guid UserId, Guid CommunityId, Guid SenderId) : IRequest;