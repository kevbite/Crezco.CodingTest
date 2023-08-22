using Crezco.CodingTest.Api.Location.IpGeoLocation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Crezco.CodingTest.Api.Location;

public static class LocationEndpoints
{
    public static void MapLocations(this IEndpointRouteBuilder webApplication)
    {
        webApplication.MapGet("/location",
            async ([FromQuery] string ip,
                [FromServices] IpValidator ipValidator,
                [FromServices] IRequestClient<GetIpLocation> client) =>
            {
                if (!ipValidator.IsValid(ip))
                {
                    return Results.Problem("Invalid IP address", statusCode: 400);
                }

                var response =
                    await client.GetResponse<GetIpLocationResult>(new GetIpLocation(ip),
                        timeout: RequestTimeout.After(5));

                return TypedResults.Ok(new LocationResourceRepresentation(
                    response.Message.CountryCode2,
                    response.Message.CountryCode3,
                    response.Message.CountryName,
                    response.Message.City
                ));
            });
    }
}

sealed record LocationResourceRepresentation(
    string CountryCode2,
    string CountryCode3,
    string CountryName,
    string City
);