using System.Net.Http.Headers;
using System.Text;
using Wonderlust.UI.Application.SessionManager;
using Wonderlust.UI.Domain.Entities;
using SerializerLib.Json;
using Wonderlust.UI.Application.Services.Communities.Requests;

namespace Wonderlust.UI.Application.Services.Communities;

public class CommunityService(HttpClient httpClient, SessionManager.SessionManager sessionManager) : ICommunityService
{
    public async Task<IEnumerable<Community>> GetCommunities()
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await httpClient.GetAsync("communities");
        var communities =
            JsonSerializer.Deserialize<List<Community>>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        return communities ?? [];
    }

    public async Task<Community> AddCommunityAsync(Community community)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new StringContent
        (
            JsonSerializer.Serialize(new AddCommunityRequest(community.Name, community.Description)),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync("communities", content);

        return JsonSerializer.Deserialize<Community>(await response.Content.ReadAsStringAsync())!;
    }

    public async Task<Community> UpdateCommunityAsync(Community community)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var content = new StringContent
        (
            JsonSerializer.Serialize(new AddCommunityRequest(community.Name, community.Description)),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PutAsync($"communities/{community.Id}", content);
        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<Community>(await response.Content.ReadAsStringAsync())!;
    }

    public async Task DeleteCommunityAsync(Guid communityId)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await httpClient.DeleteAsync($"communities/{communityId}");
        response.EnsureSuccessStatusCode();
    }
}