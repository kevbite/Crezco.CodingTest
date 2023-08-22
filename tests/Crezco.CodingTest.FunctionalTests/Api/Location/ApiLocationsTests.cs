using System.Net;
using FluentAssertions;

namespace Crezco.CodingTest.FunctionalTests.Api.Location;

public sealed class ApiLocationsTests : IAsyncLifetime
{
    private readonly ApiHarness _harness;

    public ApiLocationsTests()
    {
        _harness = new ApiHarness();
    }

    [Fact]
    public async Task ShouldReturn200OkAndCorrectResponseBody()
    {
        var ip = RandomIpAddress.Next();
        var countryCode2 = "PE";
        var countryCode3 = "PER";
        var countryName = "Peru";
        var city = "Lima";

        var json = new IpGeoLocationIpGeoJsonBuilder()
            .AddIp(ip)
            .AddCountry(countryCode2, countryCode3, countryName)
            .AddCity(city)
            .Build();

        _harness.IpGeoLocationClient.SeedSuccessfulIpGeoHandler(ip, json);

        var (statusCode, jsonDocument) = await _harness.GetLocation(ip);

        statusCode.Should().Be(HttpStatusCode.OK);
        jsonDocument!.RootElement.GetProperty("countryCode2").GetString().Should().Be(countryCode2);
        jsonDocument.RootElement.GetProperty("countryCode3").GetString().Should().Be(countryCode3);
        jsonDocument.RootElement.GetProperty("countryName").GetString().Should().Be(countryName);
        jsonDocument.RootElement.GetProperty("city").GetString().Should().Be(city);
    }

    [Theory]
    [InlineData("not-an-ip-address")]
    [InlineData("256.256.256.256")]
    [InlineData("-.-.-.-")]
    public async Task ShouldReturn400BadRequestWhenIpAddressIsNotValid(string ip)
    {
        var (statusCode, jsonDocument) = await _harness.GetLocation(ip);

        statusCode.Should().Be(HttpStatusCode.BadRequest);
        jsonDocument!.RootElement.GetProperty("type").GetString().Should()
            .Be("https://tools.ietf.org/html/rfc7231#section-6.5.1");
        jsonDocument.RootElement.GetProperty("title").GetString().Should().Be("Bad Request");
        jsonDocument.RootElement.GetProperty("status").GetInt32().Should().Be(400);
        jsonDocument.RootElement.GetProperty("detail").GetString().Should().Be("Invalid IP address");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    [InlineData(HttpStatusCode.GatewayTimeout)]
    public async Task ShouldReturn503ServiceUnavailableWhenUpstreamSystemIsUnavailable(
        HttpStatusCode upstreamSystemStatusCode)
    {
        var ip = RandomIpAddress.Next();

        _harness.IpGeoLocationClient.SeedFailedIpGeoHandler(ip, upstreamSystemStatusCode);

        var (statusCode, jsonDocument) = await _harness.GetLocation(ip);

        statusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public async Task ShouldReturn503ServiceUnavailableWhenUpstreamSystemIsSlow()
    {
        var ip = RandomIpAddress.Next();

        var json = new IpGeoLocationIpGeoJsonBuilder()
            .AddIp(ip)
            .AddCountry("PE", "PER", "Peru")
            .AddCity("Lima")
            .Build();

        _harness.IpGeoLocationClient.SeedSuccessfulIpGeoHandler(ip, json, TimeSpan.FromSeconds(10));
        var (statusCode, _) = await _harness.GetLocation(ip);

        statusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public async Task
        ShouldReturn200OkAndCorrectResponseBodyWhenIpLocationHasBeenPreviouslyCachedAndUpstreamSystemIsUnavailable()
    {
        var ip = RandomIpAddress.Next();
        var countryCode2 = "PE";
        var countryCode3 = "PER";
        var countryName = "Peru";
        var city = "Lima";

        var json = new IpGeoLocationIpGeoJsonBuilder()
            .AddIp(ip)
            .AddCountry(countryCode2, countryCode3, countryName)
            .AddCity(city)
            .Build();

        _harness.IpGeoLocationClient.SeedSuccessfulIpGeoHandler(ip, json);
        await _harness.GetLocation(ip);
        _harness.IpGeoLocationClient.SeedFailedIpGeoHandler(ip, HttpStatusCode.ServiceUnavailable);

        var (statusCode, jsonDocument) = await _harness.GetLocation(ip);

        statusCode.Should().Be(HttpStatusCode.OK);
        jsonDocument!.RootElement.GetProperty("countryCode2").GetString().Should().Be(countryCode2);
        jsonDocument.RootElement.GetProperty("countryCode3").GetString().Should().Be(countryCode3);
        jsonDocument.RootElement.GetProperty("countryName").GetString().Should().Be(countryName);
        jsonDocument.RootElement.GetProperty("city").GetString().Should().Be(city);
    }

    [Fact]
    public async Task
        ShouldReturn200OkAndCorrectResponseBodyWithLatestWhenIpLocationHasBeenPreviouslyCachedAndUpstreamSystemIsUnavailable()
    {
        var ip = RandomIpAddress.Next();
        _harness.IpGeoLocationClient.SeedSuccessfulIpGeoHandler(ip, new IpGeoLocationIpGeoJsonBuilder()
            .AddIp(ip)
            .AddCountry("PE", "PER", "Peru")
            .AddCity("Lima")
            .Build());
        await _harness.GetLocation(ip);

        var countryCode2 = "GB";
        var countryCode3 = "GBR";
        var countryName = "United Kingdom of Great Britain and Northern Ireland (the)";
        var city = "London";

        var json = new IpGeoLocationIpGeoJsonBuilder()
            .AddIp(ip)
            .AddCountry(countryCode2, countryCode3, countryName)
            .AddCity(city)
            .Build();
        _harness.IpGeoLocationClient.SeedSuccessfulIpGeoHandler(ip, json);
        await _harness.GetLocation(ip);
        
        _harness.IpGeoLocationClient.SeedFailedIpGeoHandler(ip, HttpStatusCode.ServiceUnavailable);

        var (statusCode, jsonDocument) = await _harness.GetLocation(ip);

        statusCode.Should().Be(HttpStatusCode.OK);
        jsonDocument!.RootElement.GetProperty("countryCode2").GetString().Should().Be(countryCode2);
        jsonDocument.RootElement.GetProperty("countryCode3").GetString().Should().Be(countryCode3);
        jsonDocument.RootElement.GetProperty("countryName").GetString().Should().Be(countryName);
        jsonDocument.RootElement.GetProperty("city").GetString().Should().Be(city);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
        => await _harness.DisposeAsync();
}