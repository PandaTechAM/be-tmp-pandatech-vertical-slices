using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using PandaWebApi.Services.Interfaces;

namespace PandaWebApi.Extensions;

public static class HangfireExtension
{
    private static void RunJobs() //todo just for illustration
    {
        if (true) return;
        RecurringJob.AddOrUpdate<ITestJob>("Server User Check Job", test => test.ServerUserCheckAsync(),
            Cron.Daily(23, 33));
        RecurringJob.AddOrUpdate<ITestJob>("Archive Database Job", test => test.ArchiveDatabaseAsync(),
            Cron.Minutely);
        RecurringJob.AddOrUpdate<ITestJob>("Try Close Accounts Job", test => test.TryCloseAccountsAsync(),
            Cron.Minutely);
    }

    public static WebApplicationBuilder AddHangfireServer(this WebApplicationBuilder builder)
    {
        var postgresConnectionString = builder.Configuration.GetConnectionString("Postgres");
        builder.Services.AddHangfire(configuration =>
        {
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            configuration.UseSimpleAssemblyNameTypeSerializer();
            configuration.UseRecommendedSerializerSettings();
            configuration.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(postgresConnectionString));
        });

        builder.Services.AddHangfireServer(options =>
        {
            options.WorkerCount = 5;
        });
        return builder;
    }

    public static WebApplication UseHangfireServer(this WebApplication app)
    {
        var user = app.Configuration["Security:Hangfire:Username"];
        var pass = app.Configuration["Security:Hangfire:Password"];

        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            DashboardTitle = "JobMaster Dashboard",
            Authorization = new[]
            {
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = user,
                    Pass = pass
                }
            }
        });
        app.MapHangfireDashboard();

        RunJobs();

        return app;
    }
}