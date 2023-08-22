namespace Crezco.CodingTest.FunctionalTests.Api.Location;

public class IpGeoLocationIpGeoJsonBuilder
{
    private string? _ip;
    private string? _countryCode2;
    private string? _countryCode3;
    private string? _countryName;
    private string? _city;

    public IpGeoLocationIpGeoJsonBuilder AddIp(string ip)
    {
        _ip = ip;
        
        return this;
    }
    
    public IpGeoLocationIpGeoJsonBuilder AddCountry(string code2, string code3, string name)
    {
        _countryCode2 = code2;
        _countryCode3 = code3;
        _countryName = name;
        
        return this;
    }


    public IpGeoLocationIpGeoJsonBuilder AddCity(string city)
    {
        _city = city;
        
        return this;    }

    public string Build()
    {
        return $@"{{
 ""ip"": ""{_ip}"",
 ""hostname"": ""192.168.7.1"",
 ""continent_code"": ""NA"",
 ""continent_name"": ""North America"",
 ""country_code2"": ""{_countryCode2}"",
 ""country_code3"": ""{_countryCode3}"",
 ""country_name"": ""{_countryName}"",
 ""country_capital"": ""Washington, D.C."",
 ""state_prov"": ""Virginia"",
 ""district"": """",
 ""city"": ""{_city}"",
 ""zipcode"": ""20120"",
 ""latitude"": 38.84275,
 ""longitude"": -77.43924,
 ""is_eu"": ""false"",
 ""calling_code"": ""+1"",
 ""country_tld"": "".us"",
 ""languages"": ""en-US,es-US,haw,fr"",
 ""country_flag"": ""https://ipgeolocation.io/static/flags/us_64.png"",
 ""isp"": ""Various Registries (Maintained by ARIN)"",
 ""connection_type"": """",
 ""organization"": """",
 ""asn"": ""0"",
 ""geoname_id"": 6463973,
 ""currency"": {{
 ""name"": ""US Dollar"",
 ""code"": ""USD"",
 ""symbol"": ""$""
 }},
 ""time_zone"": {{
 ""name"":""America/New_York"",
 ""offset"": -5,
 ""current_time"": ""2023-08-22 09:35:56.581-0400"",
 ""current_time_unix"": ""1.692711356581E9"",
 ""is_dst"": ""true"",
 ""dst_savings"": 1
 }}
}}";
    }

}