using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Infrastructure.Repositories;

public class MongoPostRepository(IMongoDatabase database) : IPostRepository
{
    private readonly IMongoCollection<Post> collection = database.GetCollection<Post>("Posts");

    public async Task<IEnumerable<Post>> GetAllAsync()
    {
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetAllByUserAsync(Guid userId)
    {
        return await collection.Find(p => p.AuthorId == userId).ToListAsync();
    }

    public async Task<IEnumerable<Post>> GetAllByCommunityAsync(Guid communityId)
    {
        return await collection.Find(p => p.CommunityId == communityId).ToListAsync();
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        return await collection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Post post)
    {
        await collection.InsertOneAsync(post);
    }

    public async Task UpdateAsync(Post post)
    {
        post.UpdateLastUpdateDate(DateTimeOffset.UtcNow);
        await collection.InsertOneAsync(post);
    }

    public async Task DeleteAsync(Guid id)
    {
        await collection.DeleteOneAsync(p => p.Id == id);
    }
}