namespace Crezco.CodingTest.FunctionalTests;

public static class RandomIpAddress
{
    static readonly Random Random = new();

    public static string Next()
    {
        var octets = new int[4];
        for (var i = 0; i < 4; i++)
        {
            octets[i] = Random.Next(0, 256);
        }

        return string.Join(".", octets);
    }
}