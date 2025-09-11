using GrpcServiceServer;

using Riok.Mapperly.Abstractions;

namespace GrpcService.Mappers;

[Mapper(UseDeepCloning = true)]
public static partial class HelloMapper
{
    public static partial HelloRequestDto ToDto(this HelloRequest model);

    public static HelloReplyDto ToDto(this HelloReply model)
    {
        return new HelloReplyDto
        {
            Message = model.Message,
            Addresses = model.Addresses,
            TestRequest = model.TestRequest.ToDto()
        };
    }
}

public class HelloRequestDto
{
    public string? Name { get; set => field = value?.ToUpperInvariant(); }
    public TestRequestDto? TestRequest { get; set; }
}

public class HelloReplyDto
{
    public string? Message { get; set => field = value?.ToUpperInvariant(); }
    public IList<string> Addresses { get; set; }
    public TestRequestDto? TestRequest { get; set; }
}