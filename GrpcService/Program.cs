using GrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add gRPC service
builder.Services.AddGrpc(options =>
       {
           options.MaxReceiveMessageSize = 20 * 1024 * 1024; // 20 MB
           options.MaxSendMessageSize = 20 * 1024 * 1024; // 20 MB
       })
       .AddJsonTranscoding();
builder.Services.AddGrpcReflection();

// https://medium.com/geekculture/build-high-performance-services-using-grpc-and-net7-7c0c434abbb0
// https://github.com/protocolbuffers/protobuf/blob/main/src/google/protobuf/descriptor.proto
// https://buf.build/googleapis/googleapis/file/main:google/api/annotations.proto
// https://learn.microsoft.com/en-us/aspnet/core/grpc/json-transcoding-openapi?view=aspnetcore-9.0
builder.Services.AddGrpcSwagger(); // registers Swagger generation for gRPC endpoints
builder.Services.AddSwaggerGen(); // adds the Swashbuckle generator
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IHttpContextService, HttpContextService>();

var app = builder.Build();

app.MapGrpcReflectionService();
app.MapGrpcService<GreeterService>();
app.MapGrpcService<TesterService>();
app.MapGet("/", () => "This is a gRPC server. Use a gRPC client to connect.")
   .WithName("GreeterService");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();