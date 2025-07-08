using grpcBidirectionalStreamMessageServer;
using Grpc.Core;
using Microsoft.Extensions.Logging;
namespace grpcServer.Services;

public class BidirectionalStreamMessageService : Message.MessageBase
{
    private readonly ILogger<BidirectionalStreamMessageService> _logger;

    public BidirectionalStreamMessageService(ILogger<BidirectionalStreamMessageService> logger)
    {
        _logger = logger;
    }

    public override async Task SendMessage(IAsyncStreamReader<BidirectionalStreamMessageRequest> requestStream, IServerStreamWriter<BidirectionalStreamMessageResponse> responseStream, ServerCallContext context)
    {
        Console.WriteLine("\n\n");

        await Task.Run(() =>
        {
            while (requestStream.MoveNext(context.CancellationToken).Result)
            {
                Console.WriteLine($"Request BidirectionalStreamMessage => {requestStream.Current.Name}: {requestStream.Current.Message}");
                Task.Delay(1000); // Simulate some processing delay
            }
        });

        for (int i = 0; i < 5; i++)
        {
            await responseStream.WriteAsync(new BidirectionalStreamMessageResponse
            {
                Message = $"Response {i + 1} => Ghost: Negative, sir. Out here."
            });
            await Task.Delay(1000); // Simulate some delay
        }
    }
}