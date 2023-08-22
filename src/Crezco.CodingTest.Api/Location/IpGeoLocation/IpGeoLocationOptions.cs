using System.ComponentModel.DataAnnotations;

namespace Crezco.CodingTest.Api.Location.IpGeoLocation;

public class IpGeoLocationOptions
{
    [Required(AllowEmptyStrings = false)]
    public required string ApiKey { get; init; }

    [Required]
    public required string BaseAddress { get; init; }
}