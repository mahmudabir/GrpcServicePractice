using Grpc.Core;

namespace GrpcServiceDemo.Services
{
    public class GreeterService : Greeter.GreeterBase // GreeterBase is a class that implements the server-side handling logic.
    {
        // Unary call
        public override async Task<HelloReply> SayHello(HelloRequest request,
                                                  ServerCallContext context)
        {
            return await Task.FromResult(new HelloReply
            {
                Message = $"Hello, {request.Name}"
            });
        }

        // Server streaming call
        public override async Task SayHelloStream(HelloRequest request,
                                                  IServerStreamWriter<HelloReply> responseStream,
                                                  ServerCallContext context)
        {
            for (int i = 1; i <= 5; i++)
            {
                await responseStream.WriteAsync(new HelloReply
                {
                    Message = $"Hello {request.Name}, message #{i}"
                });
                await Task.Delay(1000); // simulate delay
            }
        }
    }
}