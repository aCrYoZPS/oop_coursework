using Wonderlust.Domain.Entities;

namespace Wonderlust.Domain.Repositories;

public interface ICommunityRepository
{
    public Task<IEnumerable<Community>> GetAllAsync();
    public Task<Community> GetByIdAsync(Guid id);
    public Task AddAsync(Community community);
    public Task UpdateAsync(Community community);
    public Task DeleteAsync(Guid id);
}