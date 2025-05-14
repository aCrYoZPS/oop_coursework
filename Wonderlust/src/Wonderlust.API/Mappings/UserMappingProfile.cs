using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using Wonderlust.API.Requests.Users;
using Wonderlust.Application.Features.Users.Commands.CreateUser;
using Wonderlust.Application.Features.Users.Commands.UpdateUser;
using Wonderlust.Application.Features.Users.Dtos;
using Wonderlust.Application.Features.Users.Queries.AuthorizeUser;
using Wonderlust.Application.Security;
using Wonderlust.Domain.Entities;

namespace Wonderlust.API.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateUserRequest, CreateUserCommand>();
        CreateMap<CreateUserCommand, User>()
            .ConstructUsing(cmd => new User(cmd.Username, PasswordManager.HashPassword(cmd.Password), cmd.Email));
        CreateMap<User, UserDto>();
        CreateMap<LoginRequest, AuthorizeUserQuery>();
        CreateMap<UpdateUserRequest, UpdateUserCommand>();
    }
}