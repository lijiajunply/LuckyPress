using LuckyPress.NetData;
using LuckyPress.NetData.DataModels;
using Microsoft.EntityFrameworkCore;

namespace LuckyPress.NetService.Repositories;

public interface IEMailRepository : IAsyncDisposable
{
    Task<EMailModel[]> GetEMails();
    Task<EMailModel?> GetEMail(string id);
    Task<EMailModel> CreateOrUpdateEMail(EMailModel email);
    Task<bool> DeleteEMail(string id);
}

public class EMailRepository(IDbContextFactory<PressContext> factory): IEMailRepository
{
    private readonly PressContext context = factory.CreateDbContext();

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }

    public Task<EMailModel[]> GetEMails()
    {
        throw new NotImplementedException();
    }

    public Task<EMailModel?> GetEMail(string id)
    {
        throw new NotImplementedException();
    }

    public Task<EMailModel> CreateOrUpdateEMail(EMailModel email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteEMail(string id)
    {
        throw new NotImplementedException();
    }
}