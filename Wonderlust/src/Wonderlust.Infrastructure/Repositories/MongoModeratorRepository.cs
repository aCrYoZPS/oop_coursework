using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Infrastructure.Repositories;

public class MongoModeratorRepository(IMongoDatabase database) : IModeratorRepository
{
    private IMongoCollection<Moderator> collection = database.GetCollection<Moderator>("Moderators");

    public async Task<IEnumerable<Moderator>> GetAllAsync()
    {
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<IEnumerable<Moderator>> GetByCommunityAsync(Guid communityId)
    {
        return await collection.Find(m => m.CommunityId == communityId).ToListAsync();
    }

    public async Task<Moderator> GetByIdAsync(Guid id)
    {
        return await collection.Find(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Moderator moderator)
    {
        await collection.InsertOneAsync(moderator);
    }

    public async Task UpdateAsync(Moderator moderator)
    {
        await collection.ReplaceOneAsync(m => m.Id == moderator.Id, moderator);
    }

    public async Task DeleteAsync(Guid id)
    {
        await collection.DeleteOneAsync(m => m.Id == id);
    }
}