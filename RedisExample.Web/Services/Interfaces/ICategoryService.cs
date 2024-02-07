using RedisExample.Web.Models;

namespace RedisExample.Web.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category?>> GetAllAsync();
    Task<Ulid?> AddAsync(CategoryRequest? category);
    Task<Category?> GetByIdAsync(Ulid? id);
    Task<Category?> GetByIdWithInMemoryAsync(Ulid? id);
    Task<Category?> GetByNameAsync(string? name);
    Task RemoveAsync(Ulid? id);
    Task UpdateAsync(Ulid? id, CategoryRequest? category);
}