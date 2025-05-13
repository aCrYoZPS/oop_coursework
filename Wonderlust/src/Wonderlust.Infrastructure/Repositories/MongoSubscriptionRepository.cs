using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Infrastructure.Repositories;

public class MongoSubscriptionRepository(IMongoDatabase database) : ISubscriptionRepository
{
    private readonly IMongoCollection<Subscription> collection = database.GetCollection<Subscription>("Subscriptions");

    public async Task<IEnumerable<Subscription>> GetAllAsync()
    {
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<IEnumerable<Subscription>> GetAllByUserAsync(Guid userId)
    {
        return await collection.Find(s => s.UserId == userId).ToListAsync();
    }

    public async Task<IEnumerable<Subscription>> GetAllByCommunityAsync(Guid communityId)
    {
        return await collection.Find(s => s.CommunityId == communityId).ToListAsync();
    }

    public async Task<Subscription?> GetByIdAsync(Guid id)
    {
        return await collection.Find(s => s.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Subscription subscription)
    {
        await collection.InsertOneAsync(subscription);
    }

    public async Task UpdateAsync(Subscription subscription)
    {
        await collection.ReplaceOneAsync(s => s.Id == subscription.Id, subscription);
    }

    public async Task DeleteAsync(Guid id)
    {
        await collection.DeleteOneAsync(s => s.Id == id);
    }
}