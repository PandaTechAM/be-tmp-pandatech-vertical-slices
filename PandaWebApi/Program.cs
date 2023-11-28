using PandaVaultClient;
using PandaWebApi.Configurations;
using PandaWebApi.Extensions;
using PandaWebApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Adding custom extensions and services
builder.AddSerilog()
    .AddCors()
    .AddPostgresContext()
    .AddHealthChecks()
    .AddSwaggerGen()
    .RegisterPandaVaultEndpoint() //optional
    .AddMicrosoftIdentity();

if (!builder.Environment.IsEnvironment("Local"))
    builder.Configuration.AddPandaVault();

builder.Services.RegisterAllCustomServices();
builder.Services.AddHttpClient();
builder.Services.AddExceptionHandler<PandaExceptionHandler>();

//ASP.NET Core default services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

//Adding ExceptionHandler lambda is due to .net 8.0.100 bug
app.UseExceptionHandler(_ => { });

//Adding custom Extensions
app.MigrateDatabase()
    .UseCors()
    .UseSwagger();

//ASP.NET Core default app.Use
app.UseHttpsRedirection();
app.UseAuthorization();

//Adding custom endpoints
app.MapIdentityApi<User>();
app.MapPandaStandardEndpoints();

app.EnsureHealthy();
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