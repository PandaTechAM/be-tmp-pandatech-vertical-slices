using Communicator.Extensions;
using DistributedCache.Extensions;
using FluentMinimalApiMapper;
using GridifyExtensions.Extensions;
using MassTransit.PostgresOutbox.Extensions;
using Pandatech.Crypto.Extensions;
using Pandatech.VerticalSlices.Context;
using Pandatech.VerticalSlices.Context.SeedDatabase.User;
using Pandatech.VerticalSlices.Features.Auth.Contracts.Authenticate;
using Pandatech.VerticalSlices.SharedKernel.Extensions;
using Pandatech.VerticalSlices.SharedKernel.Interfaces;
using ResponseCrafter.Enums;
using ResponseCrafter.Extensions;
using SharedKernel.Extensions;
using SharedKernel.Helpers;
using SharedKernel.Logging;
using SharedKernel.Logging.Middleware;
using SharedKernel.OpenApi;
using SharedKernel.Postgres.Extensions;
using SharedKernel.Resilience;
using SharedKernel.ValidatorAndMediatR;

var builder = WebApplication.CreateBuilder(args);

builder.LogStartAttempt();
AssemblyRegistry.Add(typeof(Program).Assembly);

builder.WebHost.UseKestrel(o => o.AddServerHeader = false);

var repoName = builder.Environment.GetShortEnvironmentName() + ":" + builder.Configuration.GetRepositoryName();
builder
   .ConfigureWithPandaVault()
   .AddSerilog(LogBackend.ElasticSearch)
   .AddOutboundLoggingHandler()
   .AddResponseCrafter(NamingConvention.ToSnakeCase)
   .AddOpenApi()
   .AddOpenTelemetry()
   .AddMinimalApis(AssemblyRegistry.ToArray())
   .AddControllers(AssemblyRegistry.ToArray())
   .AddMediatrWithBehaviors(AssemblyRegistry.ToArray())
   .AddResilienceDefaultPipeline()
   .AddDistributedCache(o =>
   {
      o.RedisConnectionString = builder.Configuration.GetRedisUrl();
      o.ChannelPrefix = repoName;
   })
   .AddDistributedSignalR(builder.Configuration.GetRedisUrl(), repoName + ":SignalR")
   .MapDefaultTimeZone()
   .AddCors()
   .AddPostgresContextPool<PostgresContext>(builder.Configuration.GetPostgresUrl())
   .AddGridify()
   .AddMassTransit(AssemblyRegistry.ToArray())
   .AddAes256Key(builder.Configuration.GetAesKey())
   .AddCommunicator()
   .AddHangfireServer()
   .AddHealthChecks();

builder.Services.AddOutboxInboxServices<PostgresContext>();
builder.Services.AddScoped<IRequestContext, RequestContext>();

var app = builder.Build();

app
   .UseRequestLogging()
   .UseResponseCrafter()
   .UseCors()
   .MapMinimalApis()
   .MapHealthCheckEndpoints()
   .MapPrometheusExporterEndpoints()
   .MigrateDatabase<PostgresContext>()
   .EnsureHealthy()
   .UseHangfireServer()
   .ClearAssemblyRegistry()
   .UseOpenApi()
   .SeedSystemUser()
   .MapControllers();

app.LogStartSuccess();
app.Run();

//todo Set appropriate name in github repo (ex. be-pt-pandatech-website).
//todo Make sure that this repo exists in "Github repositories.xlsx" excel file and inform the devops if not.
//todo Rename application name using PascalCase (ex. PandatechWebsite).
//todo After renaming application adjust namespaces in .csproj file and refactor namespaces in all files (hint: bulk IDE function).
//todo Configure dockerfile using application name (ex. PandatechWebsite).
//todo Delete unrelated services. For example you might not need RMQ or Redis in this project.
//todo Update all Nuget packages.
//todo Change configurations (appsettings).
//todo Update ReadMe.md file.