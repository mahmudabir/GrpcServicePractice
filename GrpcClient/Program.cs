using Grpc.Core;
using Grpc.Net.Client;

using GrpcServiceDemo;

var channel = GrpcChannel.ForAddress("http://localhost:5001");
var client = new Greeter.GreeterClient(channel);

// Unary call
var reply = await client.SayHelloAsync(new HelloRequest { Name = "Abir" });
Console.WriteLine(reply.Message);

// Server streaming
using var call = client.SayHelloStream(new HelloRequest { Name = "Abir" });
await foreach (var response in call.ResponseStream.ReadAllAsync())
{
    Console.WriteLine(response.Message);
}