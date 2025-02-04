using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Controller.Shared;
using FluentValidation;
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

        services.AddValidatorsFromAssembly(
            typeof(IServiceCollectionControllerExtensions).Assembly,
            includeInternalTypes: true
        );

        services.AddBlazoredLocalStorageAsSingleton().AddBlazoredSessionStorageAsSingleton();

        return services;
    }

    public static IServiceCollection AddControllerLayerI18nText<T>(this IServiceCollection services)
        where T : class, II18nText
    {
        services.AddScoped<II18nText, T>();
        return services;
    }
}
