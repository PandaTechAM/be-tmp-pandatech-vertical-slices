using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using PandaWebApi.Configurations;

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
var app = builder.Build();

app.UseCorsToAllowAll();

app.UseSwagger();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/ping", () => "pong");

//Due to microsoft bug which is planned to be resolved in .net 8.0, we cannot see this endpoint in swagger.
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
    
app.MapControllers();
app.Run();

public abstract partial class Program { }
