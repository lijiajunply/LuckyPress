using LuckyPress.NetData;
using LuckyPress.NetData.DataModels;
using Microsoft.EntityFrameworkCore;

namespace LuckyPress.NetService.Repositories;

public interface IArticleRepository : IAsyncDisposable
{
    public Task<ArticleModel?> GetArticle(string path);
    public Task<ArticleModel> CreateOrUpdateArticle(ArticleModel article);
    public Task<bool> DeleteArticle(string path);
    public Task<ArticleModel[]> GetArticles();
    public Task<ArticleModel[]> GetArticles(string[] state);
}

public class ArticleRepository(IDbContextFactory<PressContext> factory) : IArticleRepository
{
    private readonly PressContext context = factory.CreateDbContext();

    public async Task<ArticleModel?> GetArticle(string path)
    {
        return await context.Articles
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Path == path);
    }

    public async Task<ArticleModel> CreateOrUpdateArticle(ArticleModel article)
    {
        var articleModel = await GetArticle(article.Path);
        if (articleModel == null)
        {
            article.Id = article.GetHashKey();
            context.Articles.Add(article);
            await context.SaveChangesAsync();
            return article;
        }

        articleModel.State = article.State;
        articleModel.Content = article.Content;
        articleModel.LastWriteTime = article.LastWriteTime;
        articleModel.Title = article.Title;
        await context.SaveChangesAsync();
        return articleModel;
    }

    public async Task<bool> DeleteArticle(string path)
    {
        var articleModel = await GetArticle(path);
        if (articleModel == null)
        {
            return false;
        }

        context.Articles.Remove(articleModel);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<ArticleModel[]> GetArticles()
    {
        return await context.Articles
            .Include(x => x.Tags)
            .ToArrayAsync();
    }

    public Task<ArticleModel[]> GetArticles(string[] state)
    {
        return context.Articles
            .Include(x => x.Tags)
            .Where(x => state.Contains(x.State))
            .ToArrayAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }
}