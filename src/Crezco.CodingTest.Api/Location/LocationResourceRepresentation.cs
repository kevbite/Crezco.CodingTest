namespace Crezco.CodingTest.Api.Location;

sealed record LocationResourceRepresentation(
    string CountryCode2,
    string CountryCode3,
    string CountryName,
    string City
);