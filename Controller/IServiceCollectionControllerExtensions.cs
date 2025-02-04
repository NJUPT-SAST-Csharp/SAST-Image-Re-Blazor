using Controller.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Controller;

public static class IServiceCollectionControllerExtensions
{
    public static IServiceCollection AddMediatRControllers(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(
                typeof(IServiceCollectionControllerExtensions).Assembly
            );
        });

        services.AddSingleton<ICommandSender, InnerCommandSender>();
        services.AddSingleton<IQuerySender, InnerQuerySender>();

        return services;
    }
}
