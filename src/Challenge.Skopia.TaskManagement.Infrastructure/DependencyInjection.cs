namespace Microsoft.Extensions.DependencyInjection;

using Challenge.Skopia.TaskManagement.Application.Interfaces.Persistences;
using Challenge.Skopia.TaskManagement.Infrastructure.Datas.Contexts;
using Challenge.Skopia.TaskManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class DependencyInjection
{
    public static void AddTaskManagementInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TaskManagementContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default"),
                builder => builder.MigrationsAssembly(typeof(TaskManagementContext).Assembly.FullName)), ServiceLifetime.Scoped);

        services.AddScoped<ICurrentSessionProvider, CurrentSessionProvider>();
        //services.AddNpgsql<TaskManagementContext>(configuration.GetConnectionString("Default")!, config =>
        //{
        //    config.EnableRetryOnFailure(3);
        //    config.MigrationsAssembly(typeof(TaskManagementContext).Assembly.FullName);
        //    config.MigrationsHistoryTable("_MigrationHistory".ToSnakeCase(), "blu");
        //}, options =>
        //{
        //    options.UseLazyLoadingProxies(); //Fonte: https://learn.microsoft.com/pt-br/ef/core/querying/related-data/lazy e https://learn.microsoft.com/pt-br/ef/core/performance/efficient-querying#beware-of-lazy-loading
        //    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution); //Fonte: https://learn.microsoft.com/pt-br/ef/core/querying/tracking#identity-resolution e https://learn.microsoft.com/pt-br/ef/core/querying/tracking#configuring-the-default-tracking-behavior)
        //    //options.EnableSensitiveDataLogging();
        //});

        services.TryAddScoped<ITaskManagementContext>(provider => provider.GetRequiredService<TaskManagementContext>());
    }
}