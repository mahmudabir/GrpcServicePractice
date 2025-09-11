using Grpc.Core;

using GrpcService.Mappers;

using GrpcServiceServer;

namespace GrpcService.Services
{
    public class GreeterService : Greeter.GreeterBase // GreeterBase is a class that implements the server-side handling logic.
    {
        private readonly IHttpContextService _httpContextService;

        public GreeterService(IHttpContextService httpContextService)
        {
            _httpContextService = httpContextService;
        }

        // Unary call
        public override async Task<HelloReply> SayHello(HelloRequest request,
                                                        ServerCallContext context)
        {
            var response = new HelloReply
            {
                Message = $"Hello, {request.Name}",
                Addresses =
                {
                    "Dhaka",
                    "Sherpur"
                },
                TestRequest = request.TestRequest
            };

            var requestDto = request.ToDto();
            var responseDto = response.ToDto();

            var userAgent = _httpContextService.GetHeaderValue("User-Agent");
            Console.WriteLine("Current User-Agent: " + userAgent);
            Console.WriteLine("Current Request: " + request);
            Console.WriteLine("Current Request Payload: " + requestDto.Name + "; "+ requestDto.TestRequest?.Name);
            Console.WriteLine("Current Response: " + response);
            Console.WriteLine("Current Response Payload: " + responseDto.Message + "; " + string.Join(", ", responseDto.Addresses.Select(x => x)));

            return await Task.FromResult(response);
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