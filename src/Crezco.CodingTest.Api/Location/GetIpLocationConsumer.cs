using Crezco.CodingTest.Api.Location.IpGeoLocation;
using MassTransit;

namespace Crezco.CodingTest.Api.Location;

public class GetIpLocationConsumer : IConsumer<GetIpLocation>
{
    private readonly IpGeoLocationClient _client;

    public GetIpLocationConsumer(IpGeoLocationClient client)
    {
        _client = client;
    }

    public async Task Consume(ConsumeContext<GetIpLocation> context)
    {
        var response = await _client.GetLocation(new IpGeoLocationRequest(context.Message.Ip));

        await context.RespondAsync<GetIpLocationResult>(new(
            response.Ip,
            response.CountryCode2,
            response.CountryCode3,
            response.CountryName,
            response.City
        ));
    }
}