using MediatR;
using Wonderlust.Application.Features.Communities.Dtos;

namespace Wonderlust.Application.Features.Communities.Commands.UpdateCommunity;

public record UpdateCommunityCommand() : IRequest<CommunityDto>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid CommunityId { get; set; }
    public Guid SenderId { get; set; }
}