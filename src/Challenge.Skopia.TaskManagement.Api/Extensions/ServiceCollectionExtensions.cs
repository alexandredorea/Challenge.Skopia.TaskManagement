namespace Microsoft.Extensions.DependencyInjection;

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddVersioningApi(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = ApiVersion.Default;

            options.ReportApiVersions = true;

            options.AssumeDefaultVersionWhenUnspecified = true;

            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwaggerDocument(this IServiceCollection services)
    {
        services.ConfigureOptions<SwaggerGeneratorConfiguration>();
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();

            options.CustomSchemaIds(tipo => tipo.FullName);

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath, true);
        });

        return services;
    }
}

public static class ApplicationBuilderExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UserSwaggerDocument(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DefaultModelsExpandDepth(-1);

            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.yaml", $"Desafio Bludata - {description.GroupName.ToUpperInvariant()} ");
                options.InjectStylesheet($"../assets/css/SwaggerStyle.css"); //TODO: Adicionar num CDN não seria melhor?
                options.InjectJavascript($"../assets/js/docs.js");
            }
        });

        return app;
    }
}

/// <summary>
///
/// </summary>
internal sealed class SwaggerGeneratorConfiguration : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;

    /// <summary>
    ///
    /// </summary>
    /// <param name="provider"></param>
    public SwaggerGeneratorConfiguration(IApiVersionDescriptionProvider provider)
    {
        this.provider = provider;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Version = description.ApiVersion.ToString(),
            Title = $"API do Desafio Skopia.",
            Description = "API de serviços do desafio Skopia. ",
            Contact = new OpenApiContact()
            {
                Name = "Skopia",
                Email = "vaga@bludata.com.br",
                Url = new Uri("https://skopiadigital.com.br/")
            },
            License = new OpenApiLicense()
            {
                Name = $"© Copyright {DateTime.Now.Year} Alexandre Dórea. All rights reserved."
            },
            //TermsOfService = new Uri("https://skopiadigital.com.br/"),
        };

        if (description.IsDeprecated)
        {
            info.Description = "Esta versão da API está preterida/obsoleta. Use uma das novas APIs disponíveis no explorer.";
        }

        return info;
    }
}

internal sealed class SwaggerDefaultValues : IOperationFilter
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated();

        if (operation.Parameters is null)
            return;

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            parameter.Description ??= description.ModelMetadata?.Description;

            if (parameter.Schema.Default is null && description.DefaultValue is not null)
            {
                parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
            }

            parameter.Required |= description.IsRequired;
        }
    }
}