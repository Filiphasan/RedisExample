using RedisExample.Web.Constants;
using RedisExample.Web.Models;
using RedisExample.Web.Services.Interfaces;

namespace RedisExample.Web.Services.Implementations;

public sealed class CategoryService(ICacheService cacheService) : ICategoryService
{
    private readonly List<Category> _categories = [];

    public async Task<IEnumerable<Category?>> GetAllAsync()
    {
        var cacheResult = await cacheService.GetAsync<List<Category>>(CacheKeyConstant.Category.GetAllCategories);
        if (cacheResult is { Count: > 0 })
        {
            return cacheResult;
        }

        await cacheService.SetAsync(CacheKeyConstant.Category.GetAllCategories, _categories, TimeSpan.FromDays(1));
        return _categories;
    }

    public async Task<Ulid?> AddAsync(CategoryRequest? category)
    {
        ArgumentNullException.ThrowIfNull(category);
        var newCategory = new Category
        {
            Id = Ulid.NewUlid(),
            Name = category.Name,
            Description = category.Description
        };
        _categories.Add(newCategory);

        await cacheService.SetAsync(string.Format(CacheKeyConstant.Category.GetCategoryById, newCategory.Id), newCategory, TimeSpan.FromDays(1));
        await cacheService.SetAsync(string.Format(CacheKeyConstant.Category.GetCategoryByName, newCategory.Name), newCategory, TimeSpan.FromDays(1));
        await cacheService.SetAsync(CacheKeyConstant.Category.GetAllCategories, _categories, TimeSpan.FromDays(1));
        return newCategory.Id;
    }

    public async Task<Category?> GetByIdAsync(Ulid? id)
    {
        ArgumentNullException.ThrowIfNull(id);
        var cacheKey = string.Format(CacheKeyConstant.Category.GetCategoryById, id);
        var cacheResult = await cacheService.GetAsync<Category>(cacheKey);
        if (cacheResult is not null)
        {
            return cacheResult;
        }

        var category = _categories.Find(x => x.Id == id);
        if (category is null)
        {
            return null;
        }

        await cacheService.SetAsync(cacheKey, category, TimeSpan.FromDays(1));
        return category;
    }

    public async Task<Category?> GetByNameAsync(string? name)
    {
        ArgumentNullException.ThrowIfNull(name);
        var cacheKey = string.Format(CacheKeyConstant.Category.GetCategoryByName, name);
        var cacheResult = await cacheService.GetAsync<Category>(cacheKey);
        if (cacheResult is not null)
        {
            return cacheResult;
        }

        var category = _categories.Find(x => x.Name == name);
        await cacheService.SetAsync(cacheKey, category, TimeSpan.FromDays(1));
        return category;
    }

    public async Task RemoveAsync(Ulid? id)
    {
        ArgumentNullException.ThrowIfNull(id);
        var category = _categories.Find(x => x.Id == id);
        if (category is null)
        {
            return;
        }

        _categories.Remove(category);
        await cacheService.RemoveAsync(string.Format(CacheKeyConstant.Category.GetCategoryById, id));
        await cacheService.RemoveAsync(string.Format(CacheKeyConstant.Category.GetCategoryByName, category.Name));
        await cacheService.SetAsync(CacheKeyConstant.Category.GetAllCategories, _categories, TimeSpan.FromDays(1));
    }

    public async Task UpdateAsync(Ulid? id, CategoryRequest? category)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(category);
        var categoryToUpdate = _categories.Find(x => x.Id == id);
        if (categoryToUpdate is null)
        {
            ArgumentNullException.ThrowIfNull(categoryToUpdate);
        }

        categoryToUpdate.Name = category.Name;
        categoryToUpdate.Description = category.Description;

        await cacheService.SetAsync(string.Format(CacheKeyConstant.Category.GetCategoryById, id), categoryToUpdate, TimeSpan.FromDays(1));
        await cacheService.SetAsync(string.Format(CacheKeyConstant.Category.GetCategoryByName, category.Name), categoryToUpdate, TimeSpan.FromDays(1));
        await cacheService.SetAsync(CacheKeyConstant.Category.GetAllCategories, _categories, TimeSpan.FromDays(1));
    }
}