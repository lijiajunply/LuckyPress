using LuckyPress.NetData.DataModels;
using LuckyPress.NetService.Repositories;

namespace LuckyPress.NetService.Services;

public interface IJwtHelper
{
    public string GetMemberToken(UserModel model);
}

public interface ILoginService
{
    public Task<string> Login(UserModel user);
}

public class LoginService(IJwtHelper jwt, IUserRepository userRepo) : ILoginService
{
    public async Task<string> Login(UserModel user)
    {
        var result = await userRepo.Login(user);
        return result == null ? "" : jwt.GetMemberToken(result);
    }
}