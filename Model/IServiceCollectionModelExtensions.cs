using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Refit;

[assembly: InternalsVisibleTo("Controller")]

namespace Model;

public static class IServiceCollectionModelExtensions
{
    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    private static readonly RefitSettings settings = new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(options),
    };

    public static IServiceCollection AddHttpClients(
        this IServiceCollection services,
        string baseAddress
    )
    {
        var types = typeof(IServiceCollectionModelExtensions).Assembly.GetTypes();
        types = types
            .Where(t => t.IsInterface)
            .Where(t => t.Namespace is not null && t.Namespace.Contains("Model"))
            .Where(t => t.Name.EndsWith("API", StringComparison.InvariantCultureIgnoreCase))
            .ToArray();

        for (int i = 0; i < types.Length; i++)
        {
            services
                .AddRefitClient(types[i], settings)
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new(baseAddress);
                });
        }

        return services;
    }
}
