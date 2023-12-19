namespace PandaWebApi.Services.Interfaces;

public interface ITestJob
{
    public Task ServerUserCheckAsync();
    public Task ArchiveDatabaseAsync();
    public Task TryCloseAccountsAsync();
}