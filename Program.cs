using System.Net;
using net_grpc_letsencrypt.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddLettuceEncrypt();

builder.WebHost.UseKestrel(options =>
{
    var appServices = options.ApplicationServices;
    options.Listen(IPAddress.Any, 80);
    options.Listen(IPAddress.Any, 443, listenOptions =>
    {
        // kestrel has to encrypt/decrypt certificate directly, without proxying
        listenOptions.UseHttps(h => h.UseLettuceEncrypt(appServices));
    });
});

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
