using LuckyPress.NetService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LuckyPress.NetApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TagController(ITagRepository repository) : ControllerBase
{
    [HttpGet("{name}")]
    public async Task<IActionResult> Get(string name)
    {
        var tag = await repository.GetTag(name);
        if (tag == null)
        {
            return NotFound();
        }

        return Ok(tag);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await repository.GetTags());
    }
}