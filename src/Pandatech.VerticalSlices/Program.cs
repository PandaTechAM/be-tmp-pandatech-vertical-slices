using Carter;
using Pandatech.VerticalSlices.Infrastructure;
using Pandatech.VerticalSlices.SharedKernel.Extensions;
using Pandatech.VerticalSlices.SharedKernel.SharedEndpoints;
using PandaVaultClient;
using ResponseCrafter;

var builder = WebApplication.CreateBuilder(args);

builder.LogStartAttempt();

if (!builder.Environment.IsLocal())
   builder.Configuration.AddPandaVault();

builder
   .AddInfrastructure()
   .AddCors()
   .RegisterAllServices()
   .AddSwagger()
   .AddResponseCrafter()
   .AddMediatrWithBehaviors();

builder.Services.AddCarter();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseStaticFiles();
app.UseResponseCrafter()
   .UseInfrastructure()
   .UseCors()
   .UseSwagger(app.Configuration);

app.MapPandaOptionalEndpoints();
app.MapCarter();

StartupLogger.LogStartSuccess();
app.Run();

//todo Set appropriate name in github repo (ex. be-pt-pandatech-website).
//todo Make sure that this repo exists in "Github repositories.xlsx" excel file and inform the devops if not.
//todo Rename application name using PascalCase (ex. PandatechWebsite).
//todo After renaming application adjust namespaces in .csproj file and refactor namespaces in all files (hint: bulk IDE function).
//todo Configure dockerfile using application name (ex. PandatechWebsite).
//todo Delete unrelated nuggets and services. For example you might not need RMQ or Redis in this project. Or you might not need some regex and etc.
//todo Delete health checks and other configs of unrelated services. For example you might not need RMQ or Redis in this project.
//todo Update all Nuget packages.
//todo Include all required configurations in appsettings{environment}.json.
//todo Update ReadMm.md file.

//Delete below rows if you have no integration Pandatech.VerticalSlices.Tests in your solution.

namespace Pandatech.VerticalSlices
{
   public partial class Program;
}
