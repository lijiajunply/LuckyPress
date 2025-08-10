using LuckyPress.NetData.DataModels;
using LuckyPress.NetService.Repositories;
using LuckyPress.NetService.Services;
using Microsoft.AspNetCore.Mvc;

namespace LuckyPress.NetApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserRepository repository, IJwtHelper jwt) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserModel user)
    {
        var result = await repository.Login(user);
        return result == null ? BadRequest() : Ok(new { token = jwt.GetMemberToken(result) });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserModel user)
    {
        var result = await repository.CreateOrUpdateUser(user);
        return Ok(new { token = jwt.GetMemberToken(result) });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result = await repository.GetUser(id);
        return result == null ? NotFound() : Ok(result);
    }
}