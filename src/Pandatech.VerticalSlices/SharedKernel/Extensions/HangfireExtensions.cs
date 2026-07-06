using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;

namespace Pandatech.VerticalSlices.SharedKernel.Extensions;

public static class HangfireExtensions
{
    public static WebApplicationBuilder AddHangfireServer(this WebApplicationBuilder builder)
    {
        var postgresConnectionString = builder.Configuration.GetPostgresUrl();
        builder.Services.AddHangfire(configuration =>
        {
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            configuration.UseSimpleAssemblyNameTypeSerializer();
            configuration.UseRecommendedSerializerSettings();
            configuration.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(postgresConnectionString));
        });

        builder.Services.AddHangfireServer();
        return builder;
    }

    public static WebApplication UseHangfireServer(this WebApplication app)
    {
        var user = app.Configuration.GetHangfireUsername();
        var pass = app.Configuration.GetHangfirePassword();

        app.UseHangfireDashboard("/hangfire",
            new DashboardOptions
            {
                DashboardTitle = "JobMaster Dashboard",
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = user,
                        Pass = pass
                    }
                ]
            });
        app.MapHangfireDashboard();

        return app;
    }
}
