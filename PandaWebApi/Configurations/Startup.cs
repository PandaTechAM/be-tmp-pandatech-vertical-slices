using Microsoft.EntityFrameworkCore;
using PandaWebApi.Contexts;
using StartupEnvironmentCheck;

namespace PandaWebApi.Configurations;

public class Startup : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public Startup(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        EnvironmentVariableValidator.ValidateEnvironmentVariables(RequiredEnvironments.GetEnvironmentVariables());
        
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PostgresContext>();
             dbContext.Database.Migrate();
        }
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}