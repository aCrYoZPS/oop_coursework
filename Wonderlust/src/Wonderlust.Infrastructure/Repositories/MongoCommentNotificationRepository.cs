using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Infrastructure.Repositories;

public class MongoCommentNotificationRepository(IMongoDatabase database) : ICommentNotificationRepository
{
    private readonly IMongoCollection<CommentNotification> collection =
        database.GetCollection<CommentNotification>("CommentNotifications");

    public async Task<IEnumerable<CommentNotification>> GetAllAsync()
    {
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<IEnumerable<CommentNotification>> GetByUserAsync(Guid id)
    {
        return await collection.Find(cn => cn.UserId == id).ToListAsync();
    }

    public async Task<CommentNotification?> GetByIdAsync(Guid id)
    {
        return await collection.Find(cn => cn.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(CommentNotification commentNotification)
    {
        await collection.InsertOneAsync(commentNotification);
    }

    public async Task UpdateAsync(CommentNotification commentNotification)
    {
        await collection.ReplaceOneAsync(cn => cn.Id == commentNotification.Id, commentNotification);
    }

    public async Task DeleteAsync(Guid id)
    {
        await collection.DeleteOneAsync(cn => cn.Id == id);
    }
}