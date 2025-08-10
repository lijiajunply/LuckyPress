using LuckyPress.NetData.DataModels;
using LuckyPress.NetService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LuckyPress.NetApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EMailController(IEMailRepository repository) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var email = await repository.GetEMail(id);
        if (email == null)
        {
            return NotFound();
        }

        return Ok(email);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] EMailModel email)
    {
        var newEmail = await repository.CreateOrUpdateEMail(email);
        return Ok(newEmail);
    }
}