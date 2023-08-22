using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Crezco.CodingTest.FunctionalTests.Api.Location;

public sealed class ApiHarness : IRootHarness, IAsyncDisposable
{
    private readonly ApiWebApplicationFactory _factory = new();

    public IpGeoLocationClientHarness IpGeoLocationClient { get; }
    public IServiceProvider Services => _factory.Services;

    public ApiHarness()
    {
        IpGeoLocationClient = new IpGeoLocationClientHarness(this);
    }

    public async Task<(HttpStatusCode statusCode, JsonDocument? jsonDocument)> GetLocation(string ip)
    {
        var client = _factory.CreateClient();
        var httpResponseMessage = await client.GetAsync($"/location?ip={ip}");

        var statusCode = httpResponseMessage.StatusCode;
        var jsonDocument = httpResponseMessage.Content.Headers.ContentLength > 0 ? await httpResponseMessage.Content.ReadFromJsonAsync<JsonDocument>() : null;

        return (statusCode, jsonDocument);
    }

    public async ValueTask DisposeAsync()
    {
        await _factory.DisposeAsync();
    }
}