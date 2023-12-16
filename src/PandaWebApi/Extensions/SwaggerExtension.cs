using BaseConverter;
using Microsoft.OpenApi.Models;
using PandaWebApi.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PandaWebApi.Extensions;

public static class SwaggerExtension
{
    public static WebApplicationBuilder AddPandaSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = $"{AppDomain.CurrentDomain.FriendlyName}",
                Description =
                    "Powered by PandaTech LLC: Where precision meets innovation. Let's build the future, one endpoint at a time.",
                Contact = new OpenApiContact
                {
                    Name = "PandaTech LLC",
                    Email = "info@pandatech.it",
                    Url = new Uri("https://pandatech.it"),
                }
            });

            //This option is created because due to some bug /health endpoint is not working in .NET 7. It's included in Microsoft planning.
            if(!builder.Environment.IsProduction() && !builder.Environment.IsStaging())
              options.DocumentFilter<HealthChecksFilter>();

            // Add string input support into int64 field
            options.ParameterFilter<PandaParameterBaseConverterAttribute>();
        });
        return builder;
    }

    public static WebApplication UsePandaSwagger(this WebApplication app)
    {
        if (app.Environment.IsProduction()) return app;
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            // Specify the custom display name for the tab
            options.DocumentTitle = $"Swagger - {AppDomain.CurrentDomain.FriendlyName}";

            options.InjectStylesheet("/assets/css/panda-style.css");
            options.InjectJavascript("/assets/js/docs.js");
            options.DocExpansion(DocExpansion.None);
        });
        return app;
    }
}