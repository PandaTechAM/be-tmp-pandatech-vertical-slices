using PandaVaultClient;
using PandaWebApi.Extensions;
using PandaWebApi.Models;
using ResponseCrafter;

var builder = WebApplication.CreateBuilder(args);

//Adding custom extensions and services
builder.AddSerilog()
    .AddCors()
    .AddPostgresContext()
    .AddHealthChecks()
    .RegisterAllCustomServices()
    .AddSwaggerGen()
    .AddResponseCrafter()
    .RegisterPandaVaultEndpoint() //optional
    .AddMicrosoftIdentity(); //Identity endpoints

if (!builder.Environment.IsLocal())
    builder.Configuration.AddPandaVault();

builder.Services.AddHttpClient();

//ASP.NET Core default services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

//Adding custom Extensions

app.UseResponseCrafter()
    //.EnsureHealthy()
    .MigrateDatabase()
    .UseCors()
    .UseSwagger();

//ASP.NET Core default app.Use
app.UseAuthorization();

//Adding custom endpoints
app.MapIdentityApi<User>(); //Identity endpoints
app.MapPandaStandardEndpoints();

app.MapControllers();
app.Run();

//todo Set appropriate name in github repo (ex. be-pt-pandatech-website).
//todo Make sure that this repo exists in "Github repositories.xlsx" excel file and inform the devops if not.
//todo Rename application name using PascalCase (ex. PandatechWebsite).
//todo After renaming application adjust namespaces in .csproj file and refactor namespaces in all files (hint: bulk IDE function).
//todo Configure dockerfile using application name (ex. PandatechWebsite).
//todo Delete unrelated nuggets and services. For example you might not need RMQ or Redis in this project. Or you might not need some regex and etc.
//todo Delete health checks and other configs of unrelated services. For example you might not need RMQ or Redis in this project.
//todo Update all Nuget packages.
//todo Include all required configurations in launchSettings{environment}.json.
//todo Update ReadMe.md file.

//Delete below rows if you have no integration PandaWebApi.Tests in your solution.

#pragma warning disable S1118
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
#pragma warning restore S1118
{
}