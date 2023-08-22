namespace Crezco.CodingTest.Api.Location.IpGeoLocation;

public record IpGeoLocationResponse(string Ip,
    string CountryCode2,
    string CountryCode3,
    string CountryName,
    string City
);