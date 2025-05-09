using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Infrastructure.Repositories;

public class MongoCommentReactionRepository(IMongoDatabase database) : ICommentReactionRepository
{
    private readonly IMongoCollection<CommentReaction> collection =
        database.GetCollection<CommentReaction>("CommentReactions");

    public async Task<IEnumerable<CommentReaction>> GetAllAsync()
    {
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<IEnumerable<CommentReaction>> GetAllByCommentAsync(Guid commentId)
    {
        return await collection.Find(cr => cr.CommentId == commentId).ToListAsync();
    }

    public async Task<CommentReaction> GetByIdAsync(Guid id)
    {
        return await collection.Find(cr => cr.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(CommentReaction commentReaction)
    {
        await collection.InsertOneAsync(commentReaction);
    }

    public async Task UpdateAsync(CommentReaction commentReaction)
    {
        await collection.ReplaceOneAsync(cr => cr.Id == commentReaction.Id, commentReaction);
    }

    public async Task DeleteAsync(Guid id)
    {
        await collection.DeleteOneAsync(cr => cr.Id == id);
    }
}