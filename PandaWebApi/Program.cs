using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PandaWebApi;
using PandaWebApi.Contexts;
using PandaWebApi.Extensions;
using PandaWebApi.Extensions.Health;

var builder = WebApplication.CreateBuilder(args);

//todo Set appropriate name in github repo (ex. be-pt-pandatech-website).
//todo Make sure that this repo exists in "Github repositories.xlsx" excel file and inform the devops if not.
//todo Rename application name using PascalCase (ex. PandatechWebsite).
//todo After renaming application adjust namespaces in .csproj file and refactor namespaces in all files (hint: bulk IDE function).
//todo Configure dockerfile using application name (ex. PandatechWebsite).
//todo Delete unrelated nuggets and services. For example you might not need RMQ or Redis in this project. Or you might not need some regex and etc.
//todo Delete health checks and other configs of unrelated services. For example you might not need RMQ or Redis in this project.
//todo Update all Nuget packages.
//todo Include all required env variables in launchSettings.json and RequiredEnvironments.cs.
//todo Review logger configurations. (ex. be-pt-pandatech-website)
//todo Set database connection and other environment variables. Set same env variable hardcoded in MigrationCreate project.
//todo Update ReadMe.md file.

//Adding custom extensions and services
builder.AddSerilog();
builder.AddExceptionHandler();
builder.AddCors();
builder.AddPostgresContext();
builder.AddHealthChecks();
builder.AddSwaggerGen();
builder.Services.AddHostedService<Startup>();
builder.Services.AddHttpClient();
builder.Services.RegisterAllCustomServices();
builder.Services.AddSingleton<ForTempTests>();

//Adding Microsoft Identity
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<PostgresContext>();

//ASP.NET Core default services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseHttpsRedirection();
app.UseAuthorization();

//Adding identity and other custom endpoints
app.AddPingEndpoint();
app.AddDatabaseResetEndpoints();
app.AddHealthCheckEndpoint();
app.MapIdentityApi<IdentityUser>();
app.MapGet("/test" , (ForTempTests tests) => tests.Test());

app.MapControllers();
app.Run();

//Delete below rows if you have no integration PandaWebApi.Tests in your solution.

#pragma warning disable S1118
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
#pragma warning restore S1118
{
}