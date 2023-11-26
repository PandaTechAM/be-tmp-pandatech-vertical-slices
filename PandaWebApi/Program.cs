using Microsoft.AspNetCore.Identity;
using PandaVaultClient;
using PandaWebApi.Configurations;
using PandaWebApi.Contexts;
using PandaWebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Adding custom extensions and services
builder.AddSerilog();
builder.AddCors();
builder.AddPostgresContext();
builder.AddHealthChecks();
builder.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.RegisterAllCustomServices();
builder.Services.AddExceptionHandler<PandaExceptionHandler>();

//Adding Microsoft Identity
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<PostgresContext>();

//ASP.NET Core default services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

//Adding PandaVaultClient
if (!app.Environment.IsEnvironment("Local"))
    app.RunPandaVaultClient();

//Adding ExceptionHandler lambda is due to .net 8.0.100 bug
app.UseExceptionHandler(_ => { });

//Adding custom Extensions
app.MigrateDatabase();
app.UseCors();
app.UseSwagger();

//Adding Microsoft Identity
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();

//Adding custom endpoints
app.MapPingApi();
app.MapDatabaseResetApi();
app.MapHealthApi();
app.MapPandaVaultApi();
//app.MapErrorApi(); for testing only
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