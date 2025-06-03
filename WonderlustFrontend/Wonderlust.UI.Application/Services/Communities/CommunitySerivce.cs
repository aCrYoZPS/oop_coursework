using Wonderlust.UI.Application.Services.Communities;
using Wonderlust.UI.Domain.Entities;

public class CommunityService(HttpClient httpClient) : ICommunityService
{
    public async Task<IEnumerable<Community>> GetCommunities()
    {
        return
        [
            new Community(Guid.Parse("874325f5-3994-4c5f-a81e-4f9a836b689a"), "community", "DESC",
                Guid.Parse("a98e1225-3916-4c79-9775-7d9a737c5027")),
            new Community("Other com", "No desc ahahahahahhahah",
                Guid.Parse("a98e1225-3916-5c79-9775-7d9a737c5027")),
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