using Microsoft.Extensions.DependencyInjection;
using PRN232_EbayClone.Application.Behaviors;
using System.Reflection;

namespace PRN232_EbayClone.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(RequestLoggingPipelineBahavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationPipelineBehavior<,>));

        return services;
    }
}
