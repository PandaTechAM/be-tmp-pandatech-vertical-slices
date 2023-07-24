using Microsoft.EntityFrameworkCore;
using WebApiPostgres.Configurations;
using WebApiPostgres.Contexts;

var builder = WebApplication.CreateBuilder(args);

//todo Set appropriate name in github repo (ex. be-pt-pandatech-website)
//todo Make sure that this repo exists in "Github repositories.xlsx" excel file and inform the devops if not.
//todo Rename application name using PascalCase (ex. PandatechWebsite)
//todo Configure dockerfile using application name (ex. PandatechWebsite)
//todo Update all Nuget packages
//todo Set certificate path in appsettings*.json and CertificateConfiguration.cs using repo name (ex. be-pt-pandatech-website)
//todo Set ElasticSearch index name and buffer to your repo name in ConfigureLogger.cs (ex. be-pt-pandatech-website-logs*)
//todo Set database connection and other environment variables. NOTE: Environment variables are set in launchSettings.json
//todo Warning! Make sure that you will not let Env variables in debug mode which can cause migration issues.

CertificateConfiguration.CreateCertificate();
ConfigureLogger.AddSerilogWithElastic(builder);
ConfigureServiceResponse.AddExceptionHandling(builder);
CorsPolicies.AddCorsToAllowAll(builder);
SetupDatabases.AddPostgresContext(builder);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

CorsPolicies.UseCorsToAllowAll(app);

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetService<PostgresContext>();
db!.Database.Migrate();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/ping", () => "pong");

app.MapControllers();

app.Run();

public partial class Program { }
