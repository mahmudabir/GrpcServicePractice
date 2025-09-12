namespace GrpcService.Services;

public interface IHttpContextService
{
    string GetHeaderValue(string key);
}

public class HttpContextService(IHttpContextAccessor httpContextAccessor) : IHttpContextService
{

    public string GetHeaderValue(string key)
    {
        httpContextAccessor.HttpContext.Request.Headers.TryGetValue(key, out var value);
        return value.ToString();
    }
}