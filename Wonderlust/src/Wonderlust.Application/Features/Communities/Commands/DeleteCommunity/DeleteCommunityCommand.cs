using MediatR;

namespace Wonderlust.Application.Features.Communities.Commands.DeleteCommunity;

public record DeleteCommunityCommand(
    Guid CommunityId,
    Guid SenderId
) : IRequest;