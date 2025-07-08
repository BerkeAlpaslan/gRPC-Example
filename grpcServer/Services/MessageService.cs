using Grpc.Core;
using grpcMessageServer;
using grpcServer;
using Microsoft.Extensions.Logging;
using System;

namespace grpcServer.Services;

public class MessageService : Message.MessageBase
{
    private readonly ILogger<MessageService> _logger;
    public MessageService(ILogger<MessageService> logger)
    {
        _logger = logger;
    }

    public override Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
    {
        Console.WriteLine($"Request Message Name: {request.Name}\n" +
                          $"Request Message: {request.Message}\n\n");

        return Task.FromResult(new MessageResponse
        {
            Message = "Vladimir Makarov!\n\n",
        });
    }
}
