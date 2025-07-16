namespace Microsoft.Extensions.DependencyInjection;

using Challenge.Skopia.TaskManagement.Application.PipelineBehaviors;
using FluentValidation;
using MediatR;

public static class DependencyInjection
{
    public static void AddTaskManagementServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;//Assembly.GetExecutingAssembly();
        services.AddMediatR(options => options.RegisterServicesFromAssembly(assembly));

        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    }
}