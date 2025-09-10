using Grpc.Core;

using GrpcService.Mappers;

using GrpcServiceServerTest;

namespace GrpcService.Services
{
    public class TesterService : Tester.TesterBase // GreeterBase is a class that implements the server-side handling logic.
    {
        private readonly IHttpContextService _httpContextService;

        public TesterService(IHttpContextService httpContextService)
        {
            _httpContextService = httpContextService;
        }

        // Unary call
        public override async Task<TestReply> SayTest(TestRequest request,
                                                      ServerCallContext context)
        {
            var response = new TestReply
            {
                Message = $"Test, {request.Name}",
                Addresses =
                {
                    "Dhaka",
                    "Sherpur"
                }
            };

            var userAgent = _httpContextService.GetHeaderValue("User-Agent");
            Console.WriteLine("Current User-Agent: " + userAgent);
            Console.WriteLine("Current Request: " + request);
            Console.WriteLine("Current Request Payload: " + request.ToDto().Name);
            Console.WriteLine("Current Response: " + response);
            Console.WriteLine("Current Response Payload: " + response.ToDto().Message + "; " + string.Join(", ", response.ToDto().Addresses.Select(x => x)));

            return await Task.FromResult(response);
        }

        // Server streaming call
        public override async Task SayTestStream(TestRequest request,
                                                 IServerStreamWriter<TestReply> responseStream,
                                                 ServerCallContext context)
        {
            for (int i = 1; i <= 5; i++)
            {
                await responseStream.WriteAsync(new TestReply
                {
                    Message = $"Test {request.Name}, message #{i}"
                });
                await Task.Delay(1000); // simulate delay
            }
        }
    }
}