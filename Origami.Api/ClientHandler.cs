namespace Origami.Api;

public class ManagedClientHandler : HttpClientHandler {
    bool retry = true;
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        HttpResponseMessage response = await BaseSendAsync(request, cancellationToken);
        if (retry) {
            retry = !retry;
            return await SendAsync(request, cancellationToken);
        }
        return response;
    }
    internal Task<HttpResponseMessage> BaseSendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        return base.SendAsync(request, cancellationToken);
    }
}