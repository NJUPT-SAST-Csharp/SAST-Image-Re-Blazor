using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Controller.Shared;
using Microsoft.Extensions.DependencyInjection;
using Model;

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
        services.AddSingleton<IAuthTokenProvider, AuthTokenProvider>();

        services.AddBlazoredLocalStorageAsSingleton().AddBlazoredSessionStorageAsSingleton();

        return services;
    }
}
