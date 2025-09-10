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
            Addresses = model.Addresses.ToList()
        };
    }
}

public class HelloRequestDto
{
    public string? Name { get; set => field = value?.ToUpperInvariant(); }
}

public class HelloReplyDto
{
    public string? Message { get; set => field = value?.ToUpperInvariant(); }
    public List<string> Addresses { get; set; }
}