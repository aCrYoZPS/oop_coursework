using MediatR;

namespace Wonderlust.Application.Features.Users.Queries.AuthorizeUser;

public record AuthorizeUserQuery(string Email, string Password) : IRequest<string?>;