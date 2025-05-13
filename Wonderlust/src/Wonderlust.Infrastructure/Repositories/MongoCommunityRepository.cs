using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Infrastructure.Repositories;

public class MongoCommunityRepository(IMongoDatabase database) : ICommunityRepository
{
    private readonly IMongoCollection<Community> collection = database.GetCollection<Community>("Communities");

    public async Task<IEnumerable<Community>> GetAllAsync()
    {
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<Community?> GetByIdAsync(Guid id)
    {
        return await collection.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Community community)
    {
        await collection.InsertOneAsync(community);
    }

    public async Task UpdateAsync(Community community)
    {
        await collection.ReplaceOneAsync(c => c.Id == community.Id, community);
    }

    public async Task DeleteAsync(Guid id)
    {
        await collection.DeleteOneAsync(c => c.Id == id);
    }
}