using AutoMapper;
using Wonderlust.API.Requests.Posts;
using Wonderlust.Application.Features.Posts.Commands.CreatePost;
using Wonderlust.Application.Features.Posts.Commands.UpdatePost;
using Wonderlust.Application.Features.Posts.Dtos;
using Wonderlust.Domain.Entities;

namespace Wonderlust.API.Mappings;

public class PostMappingProfile : Profile
{
    public PostMappingProfile()
    {
        CreateMap<CreatePostRequest, CreatePostCommand>();
        CreateMap<CreatePostCommand, Post>()
            .ConstructUsing(cmd => new Post(cmd.Title, cmd.ImageId, cmd.Content, cmd.CommunityId, cmd.AuthorId));
        CreateMap<Post, PostDto>();
        CreateMap<(User author, Post post), PostDto>()
            .IncludeMembers(src => src.post)
            .ForMember(dest => dest.AuthorName,
                opt =>
                    opt.MapFrom(src => src.author.Username));
        CreateMap<UpdatePostRequest, UpdatePostCommand>();
    }
}