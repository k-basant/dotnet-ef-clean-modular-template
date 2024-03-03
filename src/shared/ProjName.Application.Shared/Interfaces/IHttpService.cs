namespace ProjName.Application.Shared.Interfaces;
#nullable enable
public interface IHttpService
{
    IHttpService Init(string baseUrlOrClientName);
    IHttpService BasicAuth(string username, string password);
    IHttpService SetQueryParams(object queryParams);
    IHttpService SetRequestBody(object body, string contentType);
    Task<(T? success, string? err)> InvokeHttpCall<T>(string relativePath, string requestType, bool throwErrorWhenFailed = false);
}

