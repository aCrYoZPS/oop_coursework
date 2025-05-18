using AutoMapper;
using Wonderlust.Application.Features.Moderators.Commands.AssignModerator;
using Wonderlust.Domain.Entities;

namespace Wonderlust.API.Mappings;

public class ModeratorMappingProfile : Profile
{
    public ModeratorMappingProfile()
    {
        CreateMap<AssignModeratorCommand, Moderator>()
            .ConstructUsing(cmd => new Moderator(cmd.CommunityId, cmd.UserId));
    }
}