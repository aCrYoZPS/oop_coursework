using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.Services.Communities;

public interface ICommunityService
{
    Task<IEnumerable<Community>> GetCommunities();
    Task<Community> AddCommunityAsync(Community community);
    Task<Community> UpdateCommunityAsync(Community community);
    Task DeleteCommunityAsync(Guid communityId);
}