using LuckyPress.NetData.DataModels;
using LuckyPress.NetService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LuckyPress.NetApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticleController(IArticleRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await repository.GetArticles());
    }

    [HttpGet("{path}")]
    public async Task<IActionResult> Get(string path)
    {
        var article = await repository.GetArticle(path);
        if (article == null)
        {
            return NotFound();
        }

        return Ok(article);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ArticleModel article)
    {
        var result = await repository.CreateOrUpdateArticle(article);
        return Ok(result);
    }

    [HttpDelete("{path}")]
    public async Task<IActionResult> Delete(string path)
    {
        var result = await repository.DeleteArticle(path);
        return result ? Ok() : BadRequest();
    }
}