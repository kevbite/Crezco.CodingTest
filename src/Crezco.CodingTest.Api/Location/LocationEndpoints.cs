using Crezco.CodingTest.Api.Location.IpGeoLocation;
using Microsoft.AspNetCore.Mvc;

namespace Crezco.CodingTest.Api.Location;

public static class LocationEndpoints
{
    public static void MapLocations(this IEndpointRouteBuilder webApplication)
    {
        webApplication.MapGet("/location", 
            async ([FromQuery] string ip,
            [FromServices] IpValidator ipValidator,
            [FromServices] IpGeoLocationClient client) =>
        {
            if (!ipValidator.IsValid(ip))
            {
                return Results.Problem("Invalid IP address", statusCode: 400);
            }

            var response = await client.GetLocation(new IPGeoLocationRequest(ip));

            return TypedResults.Ok(new LocationResourceRepresentation(
                response.CountryCode2,
                response.CountryCode3,
                response.CountryName,
                response.City
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
