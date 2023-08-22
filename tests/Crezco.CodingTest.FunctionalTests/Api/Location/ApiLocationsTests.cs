using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Crezco.CodingTest.FunctionalTests.Api.Location;

public sealed class ApiLocationsTests : IAsyncLifetime
{
    private readonly ApiWebApplicationFactory _factory = new();

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

        var scope = _factory.Services.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<MockIpGeoLocationClientHttpMessageHandler>();
        handler.AddIpGeoHandler(_factory.IpGeoLocationApiKey, json);

        var client = _factory.CreateClient();
        var httpResponseMessage = await client.GetAsync($"/location?ip={ip}");

        var statusCode = httpResponseMessage.StatusCode;
        var jsonDocument = await httpResponseMessage.Content.ReadFromJsonAsync<JsonDocument>();

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
        var client = _factory.CreateClient();
        var httpResponseMessage = await client.GetAsync($"/location?ip={ip}");

        var statusCode = httpResponseMessage.StatusCode;
        var jsonDocument = await httpResponseMessage.Content.ReadFromJsonAsync<JsonDocument>();

        statusCode.Should().Be(HttpStatusCode.BadRequest);
        jsonDocument!.RootElement.GetProperty("type").GetString().Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.1");
        jsonDocument.RootElement.GetProperty("title").GetString().Should().Be("Bad Request");
        jsonDocument.RootElement.GetProperty("status").GetInt32().Should().Be(400);
        jsonDocument.RootElement.GetProperty("detail").GetString().Should().Be("Invalid IP address");
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
        => await _factory.DisposeAsync();
}