using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PandaWebApi.Configurations;
using PandaWebApi.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

//todo Set appropriate name in github repo (ex. be-pt-pandatech-website).
//todo Make sure that this repo exists in "Github repositories.xlsx" excel file and inform the devops if not.
//todo Rename application name using PascalCase (ex. PandatechWebsite).
//todo Configure dockerfile using application name (ex. PandatechWebsite).
//todo Delete unrelated nuggets and services. For example you might not need RMQ or Redis in this project.
//todo Delete health checks and other configs of unrelated services. For example you might not need RMQ or Redis in this project.
//todo Update all Nuget packages.
//todo Include all required env variables in launchSettings.json and RequiredEnvironments.cs.
//todo Set repository name in the ConfigureLogger class internal variable and adjust if you think something needs to. (ex. be-pt-pandatech-website)
//todo Set database connection and other environment variables. Set same env variable hardcoded in MigrationCreate project.
//todo Update ReadMe.md file.

builder.ReadCertificatesFromEnvironments();
builder.AddSerilog();
builder.AddExceptionHandler();
builder.AddCorsToAllowAll();
builder.AddPostgresContext();
builder.AddSwaggerGen();

builder.Services.AddHostedService<Startup>();
builder.AddHealthChecks();

builder.Services.AddSingleton<DatabaseReset>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
var app = builder.Build();

app.UseCorsToAllowAll();

app.UseStaticFiles();
app.UseSwagger();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/ping", () => "pong").WithTags("Above Board");

#if DEBUG
app.MapGet("/reset-database", (DatabaseReset warning) => warning.ResetDatabase()).WithTags("Above Board");
#endif

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).WithTags("Above Board");

app.MapControllers();
app.Run();

public partial class Program
{
}