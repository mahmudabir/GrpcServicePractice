using Grpc.Core;
using Grpc.Core.Interceptors;

public class ApiKeyClientInterceptor : Interceptor
{
    private readonly string _apiKey;

    public ApiKeyClientInterceptor(string apiKey)
    {
        _apiKey = apiKey;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        // Add API key to headers
        var headers = context.Options.Headers ?? new Metadata();
        headers.Add("X-API-KEY", _apiKey);

        var options = context.Options.WithHeaders(headers);
        var newContext = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method, context.Host, options);

        return continuation(request, newContext);
    }

    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        var headers = context.Options.Headers ?? new Metadata();
        headers.Add("X-API-KEY", _apiKey);

        var options = context.Options.WithHeaders(headers);
        var newContext = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method, context.Host, options);

        return continuation(request, newContext);
    }

    // Add other call types as needed (ClientStreaming, DuplexStreaming)
}