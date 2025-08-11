using Grpc.Core;

using System.Threading.Tasks;

namespace GrpcServiceDemo.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        // Unary call
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = $"Hello, {request.Name}"
            });
        }

        // Server streaming call
        public override async Task SayHelloStream(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
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
