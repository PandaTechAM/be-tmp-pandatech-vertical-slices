using Microsoft.EntityFrameworkCore;
using PandaWebApi.Contexts;
using PandaWebApi.Services.Interfaces;

namespace PandaWebApi.Services.Implementations;

public class TestJob : ITestJob
{
    private readonly PostgresContext _context;
    private readonly ILogger<TestJob> _logger;

    public TestJob(PostgresContext context, ILogger<TestJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ServerUserCheckAsync()
    {
        _logger.LogInformation("***ServerUserCheckAsync was initiated at {Now}***", DateTime.Now);
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == 1);
        await Task.Delay(1000);
        if (user is null)
        {
            _logger.LogError("ServerUserCheckAsync was completed at {Now}. User is null", DateTime.Now);
            throw new ArgumentNullException("User is null");
        }

        _logger.LogInformation("***ServerUserCheckAsync was completed at {Now}. User name is {UserName}***", DateTime.Now,
            user.FullName);
    }

    public async Task ArchiveDatabaseAsync()
    {
        _logger.LogInformation("***ArchiveDatabaseAsync was initiated at {Now}***", DateTime.Now);

        for (var i = 0; i < 30; i++)
        {
            await Task.Delay(1000);
            _logger.LogInformation("{GB} GB of data has been archived at {Now}", i + 1, DateTime.Now);
            if (i == 29)
                _logger.LogInformation("***ArchiveDatabaseAsync was completed at {Now}***", DateTime.Now);
        }
    }

    public async Task TryCloseAccountsAsync()
    {
        _logger.LogInformation("***TryCloseAccountsAsync was initiated at {Now}***", DateTime.Now);

        Random random = new();
        var randomNumber = random.Next(1, 5);

        await Task.Delay(2000);
        if (randomNumber == 1)
        {
            _logger.LogError("***TryCloseAccountsAsync was completed at {Now}. Random number is {RandomNumber}***",
                DateTime.Now, randomNumber);
            throw new Exception("This is a failing method");
        }

        _logger.LogInformation("***TryCloseAccountsAsync was completed at {Now}.***", DateTime.Now);
    }
}