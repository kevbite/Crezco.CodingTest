using System.Net;

namespace Crezco.CodingTest.Api;

public class IpValidator
{
    public bool IsValid(string ip)
        => IPAddress.TryParse(ip, out _);
}