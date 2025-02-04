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
                .ConfigurePrimaryHttpMessageHandler<AuthenticatedHttpClientHandler>()
                .ConfigureHttpClient(client =>
                {
                    client.BaseAddress = new(baseAddress);
                });
        }

        services.AddSingleton<AuthenticatedHttpClientHandler>();

        return services;
    }
}

file sealed class AuthenticatedHttpClientHandler(IAuthTokenProvider auth) : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        string? token = await auth.GetAsync();

        if (token is not null)
            request.Headers.Add("Authorization", "Bearer " + token);

        return await base.SendAsync(request, cancellationToken);
    }
}
