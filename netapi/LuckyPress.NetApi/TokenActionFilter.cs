using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LuckyPress.NetData.DataModels;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LuckyPress.NetApi;

public class TokenActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var bearer = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(bearer) || !bearer.Contains("Bearer")) return;
        var jwt = bearer.Split(' ');
        var tokenObj = new JwtSecurityToken(jwt[1]);

        var claimsIdentity = new ClaimsIdentity(tokenObj.Claims);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        context.HttpContext.User = claimsPrincipal;
    }
}

public static class TokenHelper
{
    /// <summary>
    /// 注：此时解析出来的UserModel的Name为'{user.Name}|{user.Password}'的哈希值
    /// </summary>
    public static UserModel? GetUser(this ClaimsPrincipal? claimsPrincipal)
    {
        var claimId = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        var claimName = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        var claimRole = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        if (claimId.IsNull() || claimName.IsNull() || claimRole.IsNull())
        {
            return null;
        }

        return new UserModel()
        {
            Name = claimName!.Value,
            Id = claimId!.Value,
            Role = claimRole!.Value
        };
    }

    private static bool IsNull(this Claim? claim)
        => claim == null || string.IsNullOrEmpty(claim.Value);
}