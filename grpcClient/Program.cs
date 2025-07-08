using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using grpcServer;
using grpcMessageServer;
using grpcServerStreamMessageServer;
using grpcClientStreamMessageServer;
using grpcBidirectionalStreamMessageServer;
using Grpc.Core;
using System.Threading;

namespace grpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5082");
            CancellationTokenSource cancellationTokenSource = new();


            var greetClient = new Greeter.GreeterClient(channel);
            HelloReply helloReply = await greetClient.SayHelloAsync(new HelloRequest
            {
                Name = "Greets from Russia!"
            });
            Console.WriteLine($"\nImran Zakhaev: {helloReply.Message}\n\n");


            var messageClient = new grpcMessageServer.Message.MessageClient(channel);
            MessageResponse messageResponse = await messageClient.SendMessageAsync(new MessageRequest
            {
                Name = "Makarov",
                Message = "Remember - no Russian."
            });
            Console.WriteLine($"Message from Zakhaev Airport: {messageResponse.Message}");


            var serverStreamMessageClient = new grpcServerStreamMessageServer.Message.MessageClient(channel);
            var serverStreamMessageResponse = serverStreamMessageClient.SendMessage(new ServerStreamMessageRequest
            {
                Name = "Shepherd",
                Message = "Ghost- you copy?"
            });
            while (await serverStreamMessageResponse.ResponseStream.MoveNext(cancellationTokenSource.Token))
            {
                Console.WriteLine($"{serverStreamMessageResponse.ResponseStream.Current.Message}");
            }


            var clientStreamMessageClient = new grpcClientStreamMessageServer.Message.MessageClient(channel);
            var clientStreamMessageRequest = clientStreamMessageClient.SendMessage();
            for (int i = 0; i < 5; i++)
            {
                await clientStreamMessageRequest.RequestStream.WriteAsync(new ClientStreamMessageRequest
                {
                    Name = "Soap MacTavish",
                    Message = $"Let's get ourselves a win, yeah, Lt.?"
                });
                await Task.Delay(1000);
            }
            await clientStreamMessageRequest.RequestStream.CompleteAsync();
            Console.WriteLine((await clientStreamMessageRequest.ResponseAsync).Message);


            var bidirectionalStreamMessageClient = new grpcBidirectionalStreamMessageServer.Message.MessageClient(channel);
            var bidirectionalStreamMessageRequest = bidirectionalStreamMessageClient.SendMessage();
            await Task.Run(async () =>
            {
                for (int i = 0; i < 5; i++)
                {
                    await bidirectionalStreamMessageRequest.RequestStream.WriteAsync(new BidirectionalStreamMessageRequest
                    {
                        Name = "Captain Price",
                        Message = $"Any issues?"
                    });
                    await Task.Delay(1000);
                }
                bidirectionalStreamMessageRequest.RequestStream.CompleteAsync().Wait();
            });
            while (await bidirectionalStreamMessageRequest.ResponseStream.MoveNext(cancellationTokenSource.Token))
            {
                Console.WriteLine(bidirectionalStreamMessageRequest.ResponseStream.Current.Message);
            }
        }
    }
}