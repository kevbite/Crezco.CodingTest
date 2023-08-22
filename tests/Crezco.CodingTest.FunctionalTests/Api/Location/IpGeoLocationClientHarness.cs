using Crezco.CodingTest.Api.Location.IpGeoLocation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Crezco.CodingTest.FunctionalTests.Api.Location;

public class IpGeoLocationClientHarness
{
    private readonly IRootHarness _rootHarness;

    public IpGeoLocationClientHarness(IRootHarness rootHarness)
    {
        _rootHarness = rootHarness;
    }
    
    public void SeedIpGeoHandler(string ip, string json)
    {
        using var scope = _rootHarness.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<MockIpGeoLocationClientHttpMessageHandler>();
        var config = scope.ServiceProvider.GetRequiredService<IOptions<IpGeoLocationOptions>>();

        handler.AddIpGeoHandler(config.Value.ApiKey, ip, json);
    }
}