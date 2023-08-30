using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PandaWebApi.Configurations;
using PandaWebApi.Configurations.Health;
using PandaWebApi.Contexts;
using PandaWebApi.Helpers;
using PandaWebApi.Services.Implementations;

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

builder.ReadCertificatesFromEnvironments();
builder.AddSerilog();
builder.AddExceptionHandler();
builder.AddCors();
builder.AddPostgresContext();
builder.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddHostedService<Startup>();
builder.AddHealthChecks();

builder.Services.AddSingleton<DatabaseHelper>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/ping", () => "pong").WithTags("Above Board");

#if DEBUG
app.MapGet("/reset-database", (DatabaseHelper helper) => helper.ResetDatabase<PostgresContext>()).WithTags("Above Board");
#endif

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).WithTags("Above Board");

app.MapControllers();
app.Run();

#pragma warning disable S1118
public partial class Program
#pragma warning restore S1118
{
}