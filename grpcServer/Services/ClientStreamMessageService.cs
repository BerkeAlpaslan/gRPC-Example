using Grpc.Core;
using grpcClientStreamMessageServer;

namespace grpcServer.Services;

public class ClientStreamMessageService : Message.MessageBase
{
    private readonly ILogger<ClientStreamMessageService> _logger;
    public ClientStreamMessageService(ILogger<ClientStreamMessageService> logger)
    {
        _logger = logger;
    }
    public override async Task<ClientStreamMessageResponse> SendMessage(IAsyncStreamReader<ClientStreamMessageRequest> requestStream, ServerCallContext context)
    {
        int i = 0;
        while (await requestStream.MoveNext(context.CancellationToken))
        {
            Console.WriteLine($"Request {++i} ClientStreamMessage => {requestStream.Current.Name}: {requestStream.Current.Message}");
        }
        
        return new ClientStreamMessageResponse
        {
            Message = "\n\nFucking Hell!\n\n"
        };
    }
}