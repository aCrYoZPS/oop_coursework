using AutoMapper;
using MediatR;
using Wonderlust.Application.Exceptions;
using Wonderlust.Application.Features.Users.Dtos;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Application.Features.Moderators.Queries.GetModerators;

public class GetModeratorsQueryHandler(
    IMapper mapper,
    IUserRepository userRepository,
    IModeratorRepository moderatorRepository,
    ICommunityRepository communityRepository
) : IRequestHandler<GetModeratorsQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetModeratorsQuery request, CancellationToken cancellationToken)
    {
        var existingCommunity = await communityRepository.GetByIdAsync(request.CommunityId);

        if (existingCommunity == null)
        {
            throw new NotFoundException($"The community with id {request.CommunityId} not found");
        }

        var moderators = await moderatorRepository.GetByCommunityAsync(existingCommunity.Id);

        var userTasks = moderators
            .Select(moderator => userRepository.GetByIdAsync(moderator.CommunityId))
            .ToList();

        var users = await Task.WhenAll(userTasks);
        var subscribers = users.Select(mapper.Map<UserDto>).ToList();

        return subscribers.Where(moderator => moderator != null);
    }
}