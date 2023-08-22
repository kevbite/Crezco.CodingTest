using Crezco.CodingTest.Api.Location.IpGeoLocation;
using Crezco.CodingTest.Api.Location.Storage;

namespace Crezco.CodingTest.Api.Location;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocation(this IServiceCollection services)
    {
        services.AddSingleton<IpValidator>();
        services.AddSingleton<IpLocationStore>();

        services.AddIpGeoLocationClient();

        return services;
    }
}