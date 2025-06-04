using System.Net.Http.Headers;
using SerializerLib.Json;
using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.Services.Subscriptions;

public class SubscriptionService(HttpClient httpClient, SessionManager.SessionManager sessionManager)
    : ISubscriptionService
{
    private static List<Subscription>? subscriptions;

    public async Task<IEnumerable<Subscription>> GetSubscriptionsAsync(Guid userId)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = httpClient.GetAsync($"subscriptions/user/{userId}").GetAwaiter().GetResult();
        var content = await response.Content.ReadAsStringAsync();
        var communities =
            JsonSerializer.Deserialize<List<Community>>(content);
        subscriptions = communities?.Select(community => new Subscription(community.Id, userId)).ToList() ?? [];
        return subscriptions;
    }

    public async Task<IEnumerable<Subscription>> GetCommunitySubscriptionsAsync(Guid communityId)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = httpClient.GetAsync($"subscriptions/community/{communityId}").GetAwaiter().GetResult();
        var content = await response.Content.ReadAsStringAsync();
        var communities =
            JsonSerializer.Deserialize<List<User>>(content);
        subscriptions = communities?.Select(user => new Subscription(communityId, user.Id)).ToList() ?? [];
        return subscriptions;
    }

    public async Task SubscribeAsync(Guid communityId, Guid userId)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await httpClient.PostAsync($"subscriptions/community/{communityId}/subscribe", null);
        response.EnsureSuccessStatusCode();
        subscriptions.Add(new Subscription(communityId, userId));
    }

    public async Task UnsubscribeAsync(Guid communityId, Guid userId)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await httpClient.DeleteAsync($"subscriptions/community/{communityId}/unsubscribe");
        response.EnsureSuccessStatusCode();
        var existingSub = subscriptions.FirstOrDefault(s => s.UserId == userId && s.CommunityId == communityId);
        if (existingSub != null)
        {
            subscriptions.Remove(existingSub);
        }
    }

    public async Task<bool> IsSubscribed(Guid communityId, Guid userId)
    {
        if (subscriptions == null)
        {
            await GetSubscriptionsAsync(userId);
        }

        var existing = subscriptions?.FirstOrDefault(s =>
            s.UserId == userId && s.CommunityId == communityId);

        return existing != null;
    }
}