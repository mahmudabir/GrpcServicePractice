using GrpcServiceDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add gRPC service
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "This is a gRPC server. Use a gRPC client to connect.");

app.Run();
