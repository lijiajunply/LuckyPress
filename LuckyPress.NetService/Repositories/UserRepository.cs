using LuckyPress.NetData;
using LuckyPress.NetData.DataModels;
using Microsoft.EntityFrameworkCore;

namespace LuckyPress.NetService.Repositories;

public interface IUserRepository : IAsyncDisposable
{
    Task<UserModel?> GetUser(string id);
    Task<UserModel> CreateOrUpdateUser(UserModel user);
    Task<bool> DeleteUser(string id);
    Task<UserModel?> Login(UserModel user);
}

public class UserRepository(IDbContextFactory<PressContext> factory) : IUserRepository
{
    private readonly PressContext context = factory.CreateDbContext();

    public async Task<UserModel?> GetUser(string id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<UserModel> CreateOrUpdateUser(UserModel user)
    {
        if (string.IsNullOrEmpty(user.Id))
        {
            user.Id = user.GetHashKey();
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            context.Users.Add(user);
        }
        else
        {
            user.UpdatedAt = DateTime.Now;
            context.Users.Update(user);
            context.Entry(user).State = EntityState.Modified;
        }

        await context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteUser(string id)
    {
        var user = await GetUser(id);

        if (user == null) return false;
        context.Users.Remove(user);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<UserModel?> Login(UserModel user)
    {
        var r = await GetUser(user.Id);
        if (r == null) return null;

        r.UpdatedAt = DateTime.Now;
        r.LastLogin = DateTime.Now;

        await context.SaveChangesAsync();

        return r;
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }
}