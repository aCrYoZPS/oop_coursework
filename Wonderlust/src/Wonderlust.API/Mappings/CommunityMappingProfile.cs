using AutoMapper;
using Wonderlust.Application.Features.Communities.Commands.CreateCommunity;
using Wonderlust.Application.Features.Communities.Dtos;
using Wonderlust.Domain.Entities;
using Wonderlust.API.Requests.Communities;

namespace Wonderlust.API.Mappings;

public class CommunityMappingProfile : Profile
{
    public CommunityMappingProfile()
    {
        CreateMap<CreateCommunityRequest, CreateCommunityCommand>();
        CreateMap<CreateCommunityCommand, Community>()
            .ConstructUsing(cmd => new Community(cmd.Name, cmd.Description, cmd.CreatorId));
        CreateMap<Community, CommunityDto>();
    }
}