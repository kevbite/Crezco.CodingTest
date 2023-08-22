using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Crezco.CodingTest.Api.Location.IpGeoLocation;

public class IpGeoLocationClient
{
    private readonly HttpClient _client;
    private readonly IOptions<IpGeoLocationOptions> _options;

    public IpGeoLocationClient(HttpClient client, IOptions<IpGeoLocationOptions> options)
    {
        _client = client;
        _options = options;
    }

    public async Task<IpGeoLocationResponse> GetLocation(IPGeoLocationRequest request)
    {
        var uri = $"/ipgeo?ip={request.IpAddress}&apiKey={_options.Value.ApiKey}";

        var geoResource = await _client.GetFromJsonAsync<IpGeoResource>(uri)
                          ?? throw new InvalidOperationException("Failed to get location");

        return new IpGeoLocationResponse(
            geoResource.Ip,
            geoResource.CountryCode2,
            geoResource.CountryCode3,
            geoResource.CountryName,
            geoResource.City
        );
    }

    sealed record IpGeoResource(
        [property: JsonPropertyName("ip")] string Ip,
        [property: JsonPropertyName("country_code2")]
        string CountryCode2,
        [property: JsonPropertyName("country_code3")]
        string CountryCode3,
        [property: JsonPropertyName("country_name")]
        string CountryName,
        [property: JsonPropertyName("city")] string City
    );
}