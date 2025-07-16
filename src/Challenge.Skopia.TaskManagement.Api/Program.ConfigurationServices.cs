namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class ConfigurationServices
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        var culture = CultureInfo.CreateSpecificCulture("pt-BR");
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        
        builder.Services.AddControllers(options =>
        {
            options.RespectBrowserAcceptHeader = true;
            options.ReturnHttpNotAcceptable = true;
            options.AllowEmptyInputInBodyModelBinding = true;
        });
        builder.Services.AddHttpContextAccessor();

        //Fonte: https://medium.com/checkout-com-techblog/json-handling-in-net-2a14612e0388
        //       https://github.com/Azure/azure-functions-dotnet-worker/blob/main/samples/Configuration/Program.cs
        builder.Services.ConfigureHttpJsonOptions(option =>
        {
            option.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            option.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        builder.Services.Configure<RouteOptions>(x => x.LowercaseUrls = true)
            .Configure<ApiBehaviorOptions>(x => x.SuppressModelStateInvalidFilter = true);

        builder.Services.AddVersioningApi();
        builder.Services.AddSwaggerDocument();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddTaskManagementServices();
        builder.Services.AddTaskManagementInfrastructure(builder.Configuration);

        return builder;
    }
}