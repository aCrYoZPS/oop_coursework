using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Infrastructure.Repositories;

public class MongoPostReactionRepository(IMongoDatabase database) : IPostReactionRepository
{
    private IMongoCollection<PostReaction> collection = database.GetCollection<PostReaction>("PostReactions");

    public async Task<IEnumerable<PostReaction>> GetAllAsync()
    {
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<IEnumerable<PostReaction>> GetAllByPostAsync(Guid postId)
    {
        return await collection.Find(pr => pr.PostId == postId).ToListAsync();
    }

    public async Task<PostReaction> GetByIdAsync(Guid id)
    {
        return await collection.Find(pr => pr.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddAsync(PostReaction postReaction)
    {
        await collection.InsertOneAsync(postReaction);
    }

    public async Task UpdateAsync(PostReaction postReaction)
    {
        await collection.ReplaceOneAsync(pr => pr.Id == postReaction.Id, postReaction);
    }

    public async Task DeleteAsync(Guid id)
    {
        await collection.DeleteOneAsync(pr => pr.Id == id);
    }
}