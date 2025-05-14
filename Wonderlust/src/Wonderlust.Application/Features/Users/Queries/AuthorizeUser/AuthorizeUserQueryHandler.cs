using MediatR;
using Microsoft.Extensions.Configuration;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Security;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Users.Queries.AuthorizeUser;

public class AuthorizeUserQueryHandler(IUserRepository userRepository, IConfiguration configuration)
    : IRequestHandler<AuthorizeUserQuery, string?>
{
    public async Task<string?> Handle(AuthorizeUserQuery request, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetByEmailAsync(request.Email);
        if (existingUser == null)
        {
            throw new NotFoundException($"User with email {request.Email} not found");
        }

        return PasswordManager.VerifyPassword(request.Password, existingUser.PasswordHash)
            ? new TokenManager(configuration).GenerateJWTToken(existingUser.Id, existingUser.Email)
            : null;
    }
}