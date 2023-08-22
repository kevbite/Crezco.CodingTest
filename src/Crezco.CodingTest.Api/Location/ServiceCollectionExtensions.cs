using Crezco.CodingTest.Api.Location.IpGeoLocation;

namespace Crezco.CodingTest.Api.Location;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocation(this IServiceCollection services)
    {
        services.AddSingleton<IpValidator>();
        services.AddIpGeoLocationClient();

        return services;
    }
}