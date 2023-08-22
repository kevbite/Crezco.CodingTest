namespace Crezco.CodingTest.Api.Location;

public record GetIpLocationResult(string Ip,
    string CountryCode2,
    string CountryCode3,
    string CountryName,
    string City
);