using GrpcServiceServer;

using GrpcServiceServerTest;

using Riok.Mapperly.Abstractions;

namespace GrpcService.Mappers;

[Mapper(UseDeepCloning = true)]
public static partial class TestMapper
{
    public static partial TestRequestDto ToDto(this TestRequest model);

    public static TestReplyDto ToDto(this TestReply model)
    {
        return new TestReplyDto
        {
            Message = model.Message,
            Addresses = model.Addresses.ToList()
        };
    }
}

public class TestRequestDto
{
    public string? Name { get; set => field = value?.ToUpperInvariant(); }
}

public class TestReplyDto
{
    public string? Message { get; set => field = value?.ToUpperInvariant(); }
    public List<string> Addresses { get; set; }
}