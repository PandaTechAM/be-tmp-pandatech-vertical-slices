using Microsoft.EntityFrameworkCore;
using PandaWebApi.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<PostgresContext>(options =>
    options.UseNpgsql()
        .UseSnakeCaseNamingConvention());

var app = builder.Build();

app.Run();

// command to add migration from base repo folder
// dotnet ef migrations add --project PandaWebApi\PandaWebApi.csproj --startup-project MigrationCreate\MigrationCreate.csproj --context PandaWebApi.Contexts.PostgresContext --configuration Debug v1 --output-dir Migrations