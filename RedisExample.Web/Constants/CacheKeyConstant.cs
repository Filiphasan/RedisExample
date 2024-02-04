namespace RedisExample.Web.Constants;

public static class CacheKeyConstant
{
    private const string BaseCacheKey = "Redis-Example";

    public static class Category
    {
        public const string GetAllCategories = $"{BaseCacheKey}:Categories:GetAllCategories";
        public const string GetCategoryById = $"{BaseCacheKey}:Categories:GetCategoryById:{{0}}";
        public const string GetCategoryByName = $"{BaseCacheKey}:Categories:GetCategoryByName:{{0}}";
    }
}