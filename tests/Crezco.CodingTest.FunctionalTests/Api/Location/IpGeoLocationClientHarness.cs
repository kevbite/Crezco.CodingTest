using System.Net;
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
    
    public void SeedSuccessfulIpGeoHandler(string ip, string json)
    {
        using var scope = _rootHarness.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<MockIpGeoLocationClientHttpMessageHandler>();
        var config = scope.ServiceProvider.GetRequiredService<IOptions<IpGeoLocationOptions>>();

        handler.AddSuccessfulIpGeoHandler(config.Value.ApiKey, ip, json);
    }
    
    public void SeedFailedIpGeoHandler(string ip, HttpStatusCode httpStatusCode)
    {
        using var scope = _rootHarness.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<MockIpGeoLocationClientHttpMessageHandler>();
        var config = scope.ServiceProvider.GetRequiredService<IOptions<IpGeoLocationOptions>>();

        handler.AddFailedIpGeoHandler(config.Value.ApiKey, ip, httpStatusCode);
    }
}