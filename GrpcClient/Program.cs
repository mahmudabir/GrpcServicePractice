using Grpc.Core;
using Grpc.Net.Client;

using GrpcServiceClient;

var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions
{
    MaxReceiveMessageSize = 20 * 1024 * 1024, // 20 MB
    MaxSendMessageSize = 20 * 1024 * 1024 // 20 MB
});
var client = new Greeter.GreeterClient(channel);
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