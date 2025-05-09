using MongoDB.Driver;
using Wonderlust.Domain.Entities;
using Wonderlust.Domain.Repositories;

namespace Wonderlust.Infrastructure.Repositories;

public class MongoUserRepository(IMongoDatabase database) : IUserRepository
{
    private IMongoCollection<User> collection = database.GetCollection<User>("Users");

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await collection.Find("{}").ToListAsync();
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await collection.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await collection.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task AddAsync(User user)
    {
        await collection.InsertOneAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        await collection.ReplaceOneAsync(u => u.Id == user.Id, user);
    }

    public async Task DeleteAsync(Guid id)
    {
        await collection.DeleteOneAsync(u => u.Id == id);
    }
}