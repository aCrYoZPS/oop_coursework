using Wonderlust.Domain.Entities;

namespace Wonderlust.Domain.Repositories;

public interface IUserRepository
{
    public Task<IEnumerable<User>> GetAllAsync();
    public Task<User> GetByIdAsync(Guid id);
    public Task<User> GetByEmailAsync(string email);
    public Task AddAsync(User user);
    public Task UpdateAsync(User user);
    public Task DeleteAsync(Guid id);
}