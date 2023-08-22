using System.Net;
using System.Text;

namespace Crezco.CodingTest.FunctionalTests;

public class MockIpGeoLocationClientHttpMessageHandler : HttpMessageHandler
{
    private readonly List<Handler> _handlers = new();

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_handlers.FirstOrDefault(h => h.Match(request)) is { } handler)
        {
            return await handler.Resolve(request);
        }

        return new HttpResponseMessage(HttpStatusCode.NotFound);
    }

    private void AddHandler(Func<HttpRequestMessage, bool> match, Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
    {
        _handlers.Add(new Handler(match, handler));
    }

    public void AddSuccessfulIpGeoHandler(string apiKey, string ip, string json, TimeSpan timeToRespond)
    {
        AddIpGeoHandler(
            apiKey, ip,
            async _ =>
            {
                await Task.Delay(timeToRespond);

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                };
            });
    }

    public void AddFailedIpGeoHandler(string apiKey, string ip, HttpStatusCode httpStatusCode)
    {
        AddIpGeoHandler(
            apiKey, ip,
            _ => Task.FromResult(new HttpResponseMessage(httpStatusCode)));
    }

    private void AddIpGeoHandler(string apiKey, string ip, Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
    {
        AddHandler(
            request => request.Method == HttpMethod.Get &&
                       request.RequestUri?.PathAndQuery.StartsWith("/ipgeo", StringComparison.Ordinal) == true &&
                       request.RequestUri?.Query.Contains($"apiKey={apiKey}") == true &&
                       request.RequestUri?.Query.Contains($"ip={ip}") == true,
            handler);
    }

    sealed record Handler(Func<HttpRequestMessage, bool> Match, Func<HttpRequestMessage, Task<HttpResponseMessage>> Resolve);
}