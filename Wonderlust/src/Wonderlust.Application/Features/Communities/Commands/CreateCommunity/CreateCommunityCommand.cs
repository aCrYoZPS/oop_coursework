using MediatR;
using Wonderlust.Application.Features.Communities.Dtos;

namespace Wonderlust.Application.Features.Communities.Commands.CreateCommunity;

public record CreateCommunityCommand(
    string Title,
    string Description,
    Guid CreatorId
) : IRequest<CommunityDto>;