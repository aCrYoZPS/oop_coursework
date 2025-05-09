using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Infrastructure.Repositories;

public class MongoPostNotificationRepository(IMongoDatabase database) : IPostNotificationRepository
{
    private IMongoCollection<PostNotification> collection =
        database.GetCollection<PostNotification>("PostNotifications");

    public async Task<IEnumerable<PostNotification>> GetAllAsync()
    {
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<IEnumerable<PostNotification>> GetByUserAsync(Guid userId)
    {
        return await collection.Find(pn => pn.UserId == userId).ToListAsync();
    }

    public async Task<PostNotification> GetByIdAsync(Guid id)
    {
        return await collection.Find(pn => pn.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(PostNotification postNotification)
    {
        await collection.InsertOneAsync(postNotification);
    }

    public async Task UpdateAsync(PostNotification postNotification)
    {
        await collection.ReplaceOneAsync(pn => pn.Id == postNotification.Id, postNotification);
    }

    public async Task DeleteAsync(Guid id)
    {
        await collection.DeleteOneAsync(pn => pn.Id == id);
    }
}