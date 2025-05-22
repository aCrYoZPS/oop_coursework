using AutoMapper;
using Wonderlust.API.Requests.Comments;
using Wonderlust.Application.Features.Comments.Commands.CreateComment;
using Wonderlust.Application.Features.Comments.Commands.UpdateComment;
using Wonderlust.Application.Features.Comments.Dtos;
using Wonderlust.Domain.Entities;

namespace Wonderlust.API.Mappings;

public class CommentMappingProfile : Profile
{
    public CommentMappingProfile()
    {
        CreateMap<CreateCommentRequest, CreateCommentCommand>();
        CreateMap<CreateCommentCommand, Comment>()
            .ConstructUsing(cmd =>
                new Comment(cmd.Content, cmd.AuthorId, cmd.PostId, cmd.ParentCommentId));
        CreateMap<Comment, CommentDto>();
        CreateMap<UpdateCommentRequest, UpdateCommentCommand>();
    }
}
