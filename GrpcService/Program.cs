using GrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add gRPC service
builder.Services.AddGrpc(options =>
       {
           options.MaxReceiveMessageSize = 20 * 1024 * 1024; // 20 MB
           options.MaxSendMessageSize = 20 * 1024 * 1024; // 20 MB
       })
       .AddJsonTranscoding();
builder.Services.AddGrpcSwagger(); // registers Swagger generation for gRPC endpoints
builder.Services.AddSwaggerGen(); // adds the Swashbuckle generator
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IHttpContextService, HttpContextService>();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGrpcService<TesterService>();
app.MapGet("/", () => "This is a gRPC server. Use a gRPC client to connect.")
   .WithName("GreeterService");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();