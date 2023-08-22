using Microsoft.Extensions.Options;

namespace Crezco.CodingTest.Api.Location.IpGeoLocation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIpGeoLocationClient(this IServiceCollection services)
    {
        var httpClientBuilder = services.AddHttpClient<IpGeoLocationClient>();
        httpClientBuilder
            .ConfigureHttpClient((provider, client) =>
            {
                var options = provider.GetRequiredService<IOptions<IpGeoLocationOptions>>();

                client.BaseAddress = new Uri(options.Value.BaseAddress);
            });
        services.AddOptions<IpGeoLocationOptions>()
            .BindConfiguration("IpGeoLocation")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }
}