using Crezco.CodingTest.Api.Location.IpGeoLocation;
using Crezco.CodingTest.Api.Location.Storage;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Crezco.CodingTest.Api.Location;

public static class LocationEndpoints
{
    public static void MapLocations(this IEndpointRouteBuilder webApplication)
    {
        webApplication.MapGet("/location",
            async ([FromQuery] string ip,
                [FromServices] IpValidator validator,
                [FromServices] IpLocationStore store,
                [FromServices] IRequestClient<GetIpLocation> client) =>
            {
                if (!validator.IsValid(ip))
                {
                    return Results.Problem("Invalid IP address", statusCode: 400);
                }

                try
                {
                    var response =
                        await client.GetResponse<GetIpLocationResult>(new GetIpLocation(ip),
                            timeout: RequestTimeout.After(s: 5));

                    await store.Store(new IpLocation(ObjectId.Empty, ip,
                        new Storage.Location(response.Message.CountryCode2, response.Message.CountryCode3,
                            response.Message.CountryName, response.Message.City)));
                        
                    return TypedResults.Ok(new LocationResourceRepresentation(
                        response.Message.CountryCode2,
                        response.Message.CountryCode3,
                        response.Message.CountryName,
                        response.Message.City
                    ));
                }
                catch (RequestException)
                {
                    if (await store.Latest(ip) is { } latest)
                        return TypedResults.Ok(new LocationResourceRepresentation(
                            latest.Location.CountryCode2,
                            latest.Location.CountryCode3,
                            latest.Location.CountryName,
                            latest.Location.City
                        ));
                    
                    return Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
                }
            });
    }
}