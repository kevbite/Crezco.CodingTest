using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace Crezco.CodingTest.FunctionalTests;

public sealed class
    ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    public string IpGeoLocationApiKey { get; } = Guid.NewGuid().ToString();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("IpGeoLocation:ApiKey", IpGeoLocationApiKey);
        builder.UseSetting("IpGeoLocation:BaseAddress", "https://api.ipgeolocation.local/");
        
        builder.ConfigureTestServices(services =>
        {
            ServiceCollectionServiceExtensions.AddSingleton<MockIpGeoLocationClientHttpMessageHandler>(services);
            OptionsServiceCollectionExtensions.Configure<HttpClientFactoryOptions>(services, "IpGeoLocationClient", options =>
            {
                options.HttpMessageHandlerBuilderActions.Add(b => b.PrimaryHandler = ServiceProviderServiceExtensions.GetRequiredService<MockIpGeoLocationClientHttpMessageHandler>(b.Services));
            });
        });
        base.ConfigureWebHost(builder);
    }
}