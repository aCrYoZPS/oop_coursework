using AutoMapper;
using Wonderlust.Application.Features.Comments.Dtos;
using Wonderlust.Domain.Entities;

namespace Wonderlust.API.Mappings;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<Comment, CommentDto>();
    }
}
