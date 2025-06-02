using Wonderlust.UI.Application.Services.Communities;
using Wonderlust.UI.Domain.Entities;

public class CommunityService(HttpClient httpClient) : ICommunityService
{
    public async Task<IEnumerable<Community>> GetCommunities()
    {
        return
        [
            new Community()
            {
                Name = "COMMUNITY", Description = "DESC", CreationDate = DateTimeOffset.UtcNow, SubscriberCount = 0
            }
        ];
    }

    public async Task<Community> UpdateCommunityAsync(Community community)
    {
        return community;
    }

    public async Task<Community> AddCommunityAsync(Community community)
    {
        return community;
    }

    public async Task DeleteCommunityAsync(Guid communityId)
    {
        return;
    }
}