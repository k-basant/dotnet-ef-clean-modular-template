using System.Net.Http.Headers;

namespace ProjName.Infrastructure.Shared.Services;

public class HttpService : IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private HttpClient? _httpClient;
    private string? _queryParams;
    private HttpContent? _body;
    public HttpService(IHttpClientFactory clientFactory)
    {
        _httpClientFactory = clientFactory;
    }
    public IHttpService Init(string baseUrlOrClientName)
    {
        if (baseUrlOrClientName.StartsWith("http"))
        {
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(baseUrlOrClientName + (!baseUrlOrClientName.EndsWith(@"/") ? @"/" : string.Empty));
        }
        else
        {
            _httpClient = _httpClientFactory.CreateClient(baseUrlOrClientName);
        }
        return this;
    }
    public IHttpService BasicAuth(string username, string password)
    {
        if (_httpClient == null) throw new InvalidOperationException("Please make sure to call `Init` method first.");

        var authValue = $"{username}:{password}".ToBase64Str();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);

        return this;
    }

    public IHttpService SetQueryParams(object queryParams)
    {
        if (_httpClient == null) throw new InvalidOperationException("Please make sure to call `Init` method first.");

        _queryParams = queryParams.ToQueryString();

        return this;
    }

    public IHttpService SetRequestBody(object body, string contentType)
    {
        if (_httpClient == null) throw new InvalidOperationException("Please make sure to call `Init` method first.");

        _body = new StringContent(body.ToJsonString(), System.Text.Encoding.UTF8, contentType);

        return this;
    }

    public async Task<(T? success, string? err)> InvokeHttpCall<T>(string relativePath, string requestType, bool throwErrorWhenFailed = false)
    {
        if (_httpClient == null) throw new InvalidOperationException("Please make sure to call `Init` method first.");

        var relativeUri = $"{relativePath}{_queryParams}";
        var response = requestType switch
        {
            "Get"   => await _httpClient.GetAsync(relativeUri),
            "Post"  => await _httpClient.PostAsync(relativeUri, _body),
            _       => throw new NotImplementedException(),
        };

        bool isError = false;

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            isError = true;
            if (throwErrorWhenFailed) { throw; }
        }

        var content = await response.Content.ReadAsStringAsync();

        if (isError) return (default(T), content);
        else return (content.ParseAsTypedObject<T>(), null);
    }
}
