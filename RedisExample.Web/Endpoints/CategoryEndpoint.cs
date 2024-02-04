using Carter;
using RedisExample.Web.Models;
using RedisExample.Web.Services.Interfaces;

namespace RedisExample.Web.Endpoints;

public sealed class CategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/categories")
            .WithTags("Category");

        group.MapGet("", GetAllAsync);
        group.MapGet("/{id}", GetByIdAsync);
        group.MapGet("names/{name}", GetByNameAsync);
        group.MapPost("", AddAsync);
        group.MapPut("/{id}", UpdateAsync);
        group.MapDelete("/{id}", RemoveAsync);
    }

    private static async Task<IResult> GetAllAsync(ICategoryService categoryService)
    {
        return Results.Ok(await categoryService.GetAllAsync());
    }

    private static async Task<IResult> GetByIdAsync(Ulid id, ICategoryService categoryService)
    {
        return Results.Ok(await categoryService.GetByIdAsync(id));
    }
    
    private static async Task<IResult> GetByNameAsync(string name, ICategoryService categoryService)
    {
        return Results.Ok(await categoryService.GetByNameAsync(name));
    }

    private static async Task<IResult> AddAsync(CategoryRequest category, ICategoryService categoryService)
    {
        return Results.Ok(await categoryService.AddAsync(category));
    }

    private static async Task<IResult> UpdateAsync(Ulid id, CategoryRequest category, ICategoryService categoryService)
    {
        await categoryService.UpdateAsync(id, category);
        return Results.Ok();
    }

    private static async Task<IResult> RemoveAsync(Ulid id, ICategoryService categoryService)
    {
        await categoryService.RemoveAsync(id);
        return Results.Ok();
    }
}