using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using PandaWebApi.Contexts;

namespace WebApiPostgresTests.Configurations;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<PostgresContext>));
            services.AddDbContextPool<PostgresContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString()).UseSnakeCaseNamingConvention();
            });
        });
    }

    public async Task InitializeAsync()
    {
        SetEnvironments();
        await _dbContainer.StartAsync();
        await CreateDatabase();
        HttpClient = CreateClient();
        await InitializeRespawner();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }

    private async Task InitializeRespawner()
    {
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" }
        });
    }

    public async Task ResetStateAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }


    private async Task CreateDatabase() //not sure if this is needed
    {
        using var scope = Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<PostgresContext>();
        await context.Database.MigrateAsync();
    }

    private void SetEnvironments()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        Environment.SetEnvironmentVariable("POSTGRES_CONNECTION_STRING", _dbContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("REDIS_CONNECTION_STRING", "localhost:6379");
        Environment.SetEnvironmentVariable("RABBITMQ_EXCHANGE_NAME", "panda");
        Environment.SetEnvironmentVariable("RABBITMQ_ROUTING_KEY", "panda");
        Environment.SetEnvironmentVariable("RABBITMQ_QUEUE_NAME", "panda");
        Environment.SetEnvironmentVariable("RABBITMQ_ROUTING_KEY_DLX", "panda-dlx");
        Environment.SetEnvironmentVariable("RABBITMQ_QUEUE_NAME_DLX", "panda-dlx");
        Environment.SetEnvironmentVariable("RABBITMQ_URI", "amqp://guest:guest@localhost:5672");
        Environment.SetEnvironmentVariable("ELASTIC_SEARCH_URL", "http://localhost:9200");
        Environment.SetEnvironmentVariable("ELASTIC_INDEX_NAME", "panda");
        Environment.SetEnvironmentVariable("CORS_ALLOWED_ORIGINS", "http://localhost:3000");
        Environment.SetEnvironmentVariable("USER_MANAGEMENT_ADDRESS", "http://localhost:5000");
    }
}