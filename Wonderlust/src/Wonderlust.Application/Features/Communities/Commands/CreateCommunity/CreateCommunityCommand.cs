using MediatR;
using Wonderlust.Application.Features.Communities.Dtos;

namespace Wonderlust.Application.Features.Communities.Commands.CreateCommunity;

public class CreateCommunityCommand() : IRequest<CommunityDto>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CreatorId { get; set; }
}