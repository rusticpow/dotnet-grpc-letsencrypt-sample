using Grpc.Core;
using net_grpc_letsencrypt;

namespace net_grpc_letsencrypt.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override async Task SayHello(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
    {
        int counter = 0;
        while (true)
        {
            if (context.Status.StatusCode != StatusCode.OK)
            {
                return;
            }

            await responseStream.WriteAsync(new HelloReply
            {
                Message = $"Hello: {request.Name} - " + counter
            });
            counter++;
            await Task.Delay(1000);
        }
    }
}
