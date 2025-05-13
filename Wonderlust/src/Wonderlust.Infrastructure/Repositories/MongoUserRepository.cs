using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Infrastructure.Repositories;

public class MongoUserRepository : IUserRepository
{
    private readonly IMongoCollection<User> collection;

    public MongoUserRepository(IMongoDatabase database)
    {
        collection = database.GetCollection<User>("Users");
        ConfigureIndexes(collection);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await collection.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await collection.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task AddAsync(User user)
    {
        try
        {
            await collection.InsertOneAsync(user);
        }
        catch (MongoWriteException ex)
        {
            throw new Exception($"Failed to add new user with exception: {ex}");
        }
    }

    public async Task UpdateAsync(User user)
    {
        await collection.ReplaceOneAsync(u => u.Id == user.Id, user);
    }

    public async Task DeleteAsync(Guid id)
    {
        await collection.DeleteOneAsync(u => u.Id == id);
    }

    private static void ConfigureIndexes(IMongoCollection<User> collection)
    {
        var indexKeys = Builders<User>.IndexKeys.Ascending(u => u.Email);

        var indexOptions = new CreateIndexOptions
        {
            Unique = true,
            Collation = new Collation("en", strength: CollationStrength.Secondary)
        };

        collection.Indexes.CreateOne(
            new CreateIndexModel<User>(indexKeys, indexOptions)
        );
    }
}