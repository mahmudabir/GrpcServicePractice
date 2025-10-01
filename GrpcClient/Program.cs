using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;

using GrpcServiceClient;


#region DI with GRPC Client Factory (Optional)

//public class GrpcClientService : IGrpcClientService
//{
//    private readonly PreBookingService.PreBookingServiceClient _client;

//    public GrpcClientService(PreBookingService.PreBookingServiceClient client)
//    {
//        _client = client;
//    }

//    public async Task<BrandedFareResponse> GetBrandedFare(PreBookingRequest request)
//    {
//        // API key is automatically added by the interceptor
//        return await _client.BrandedFareAsync(request);
//    }
//}

//// Program.cs
//builder.Services.AddGrpcClient<Greeter.GreeterClient>(options =>
//{
//    options.Address = new Uri("https://localhost:7001");
//})
//.AddInterceptor(() => new ApiKeyClientInterceptor(
//    builder.Configuration["ApiKeySettings:ApiKey"]!));

//// Register your service
//builder.Services.AddScoped<IGrpcClientService, GrpcClientService>();

#endregion


var apiKey = "SK-SupplierGateway-2024-Dev-Key-Change-In-Production";
var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions
{
    MaxReceiveMessageSize = 20 * 1024 * 1024, // 20 MB
    MaxSendMessageSize = 20 * 1024 * 1024 // 20 MB
});
var invoker = channel.Intercept(new ApiKeyClientInterceptor(apiKey));
var client = new Greeter.GreeterClient(invoker);


Console.WriteLine($"Application Stared At: {DateTime.Now}");
Console.WriteLine();

const int loopCount = 10_000;
Console.WriteLine($"Loop count: {loopCount}");
Console.WriteLine($"Loop Stared At: {DateTime.Now}");
List<AsyncUnaryCall<HelloReply>> calls = new List<AsyncUnaryCall<HelloReply>>();
for (int i = 0; i < loopCount; i++)
{
    AsyncUnaryCall<HelloReply> clientCall = client.SayHelloAsync(new HelloRequest
    {
        Name = "Abir"
    }, new Metadata
        {
            { "X-API-KEY", "SK-SupplierGateway-2024-Dev-Key-Change-In-Production" }
        });
    calls.Add(clientCall);
}
Console.WriteLine($"Loop Ended At: {DateTime.Now}");
Console.WriteLine();

// Extract tasks from the calls when you need to await them
var tasks = calls.Select(call => call.ResponseAsync);
var results = await Task.WhenAll(tasks);
Console.WriteLine(string.Join("\r", results.Select(result => result.Message)));
Console.WriteLine();

Console.WriteLine($"Loop Call await Ended At: {DateTime.Now}");
Console.WriteLine();

Console.WriteLine($"Single Call Started At: {DateTime.Now}");
// Unary call
var reply = await client.SayHelloAsync(new HelloRequest
{
    Name = "Abir"
});
Console.WriteLine(reply.Message);
Console.WriteLine($"Single Call Ended At: {DateTime.Now}");
Console.WriteLine();

Console.WriteLine($"Stream Call Started At: {DateTime.Now}");
// Server streaming
using var call = client.SayHelloStream(new HelloRequest
{
    Name = "Abir"
});
await foreach (var response in call.ResponseStream.ReadAllAsync())
{
    Console.WriteLine(response.Message);
}
Console.WriteLine($"Stream Call Ended At: {DateTime.Now}");
Console.WriteLine();


Console.WriteLine($"Application Ended At: {DateTime.Now}");
Console.WriteLine();