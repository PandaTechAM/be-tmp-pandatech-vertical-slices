using Microsoft.EntityFrameworkCore;
using PandaWebApi.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<PostgresContext>(options =>
    options.UseNpgsql("Server=localhost;Port=5432;Database=panda_web_api;User Id=test;Password=test;")
        .UseSnakeCaseNamingConvention());

var app = builder.Build();

app.Run();

// command to add migration
// dotnet ef migrations add --project PandaWebApi\PandaWebApi.csproj --startup-project MigrationCreate\MigrationCreate.csproj --context PandaWebApi.Contexts.PostgresContext --configuration Debug v1 --output-dir Migrations