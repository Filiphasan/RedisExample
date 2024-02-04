namespace RedisExample.Web.Models;

public record CategoryRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}