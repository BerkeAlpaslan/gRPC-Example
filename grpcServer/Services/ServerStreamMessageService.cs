using System;
using System.Threading.Tasks;
using Grpc.Core;
using grpcServerStreamMessageServer;
using Microsoft.Extensions.Logging;

namespace grpcServer.Services;

public class ServerStreamMessageService : Message.MessageBase
{
    private readonly ILogger<ServerStreamMessageService> _logger;
    public ServerStreamMessageService(ILogger<ServerStreamMessageService> logger)
    {
        _logger = logger;
    }

    public override async Task SendMessage(ServerStreamMessageRequest request, IServerStreamWriter<ServerStreamMessageResponse> responseStream, ServerCallContext context)
    {
        Console.WriteLine($"Request ServerStreamMessage => {request.Name}: {request.Message}\n\n");
        for (int i = 0; i < 5; i++)
        {
            await responseStream.WriteAsync(new ServerStreamMessageResponse
            {
                Message = $"Response {i + 1} => Ghost: Yes, sir."
            });
            await Task.Delay(1000); // Simulate some delay
        }
    }
}