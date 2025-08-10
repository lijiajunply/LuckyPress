using LuckyPress.NetData;
using LuckyPress.NetData.DataModels;
using Microsoft.EntityFrameworkCore;

namespace LuckyPress.NetService.Repositories;

public interface ITagRepository : IAsyncDisposable
{
    Task<TagModel[]> GetTags();
    Task<TagModel?> GetTag(string name);
    Task<TagModel?> CreateOrUpdateTag(TagModel tag);
    Task<bool> DeleteTag(string name);
    Task<IEnumerable<ArticleModel>> GetArticles(string name);
}

public class TagRepository(IDbContextFactory<PressContext> factory) : ITagRepository
{
    private readonly PressContext context = factory.CreateDbContext();

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }

    public async Task<TagModel[]> GetTags()
    {
        return await context.Tags.ToArrayAsync();
    }

    public async Task<TagModel?> GetTag(string name)
    {
        return await context.Tags.FirstOrDefaultAsync(t => t.Name == name);
    }

    public async Task<TagModel?> CreateOrUpdateTag(TagModel tag)
    {
        var t = await GetTag(tag.Name);
        if (t != null)
        {
            t.Name = tag.Name;
        }
        else
        {
            await context.Tags.AddAsync(tag);
        }

        await context.SaveChangesAsync();
        return tag;
    }

    public async Task<bool> DeleteTag(string name)
    {
        var t = await GetTag(name);
        if (t == null) return false;
        context.Tags.Remove(t);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ArticleModel>> GetArticles(string name)
    {
        var a = await context.Tags
            .Include(x => x.Articles)
            .FirstOrDefaultAsync(t => t.Name == name);
        return a?.Articles ?? [];
    }
}