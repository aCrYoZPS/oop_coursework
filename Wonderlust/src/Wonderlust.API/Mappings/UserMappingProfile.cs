using AutoMapper;
using Wonderlust.API.Requests.Users;
using Wonderlust.Application.Features.Users.Commands.CreateUser;
using Wonderlust.Application.Features.Users.Dtos;
using Wonderlust.Domain.Entities;

namespace Wonderlust.API.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserRequest, CreateUserCommand>();
        CreateMap<CreateUserCommand, User>()
            .ConstructUsing(cmd => new User(cmd.Username, cmd.Password, cmd.Email));
        CreateMap<User, UserDto>();
    }
}