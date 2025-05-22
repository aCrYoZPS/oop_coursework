using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Infrastructure.Repositories;

public class MongoCommentRepository(IMongoDatabase database) : ICommentRepository
{
    private readonly IMongoCollection<Comment> collection = database.GetCollection<Comment>("Comments");

    public async Task<IEnumerable<Comment>> GetAllAsync()
    {
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<IEnumerable<Comment>> GetAllByPostAsync(Guid postId)
    {
        return await collection.Find(c => c.PostId == postId).ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(Guid id)
    {
        return await collection.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Comment comment)
    {
        await collection.InsertOneAsync(comment);
    }

    public async Task UpdateAsync(Comment comment)
    {
        comment.SetLastUpdateDate(DateTimeOffset.UtcNow);
        await collection.ReplaceOneAsync(c => c.Id == comment.Id, comment);
    }

    public async Task DeleteAsync(Guid id)
    {
        var comment = await collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        comment.SetDeleted();
        await UpdateAsync(comment);
    }
}