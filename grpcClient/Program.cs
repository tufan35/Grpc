﻿using Grpc.Core;
using Grpc.Net.Client;
using grpcMessageClient;
using grpcServer;

internal class Program
{
    static async Task Main(string[] args)
    {
       
        var channel = GrpcChannel.ForAddress("http://localhost:5296");
        var messageClient = new Message.MessageClient(channel);

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var res = messageClient.SendMessage();
        var task1 = Task.Run(async ()=>{
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                res.RequestStream.WriteAsync(new MessageRequest{Name="tufan", Message = "mesaj" +i});
            }
        });

        while (await res.ResponseStream.MoveNext(cancellationTokenSource.Token))
        {
             System.Console.WriteLine(res.ResponseStream.Current.Message);
        }

        await task1;
        await res.RequestStream.CompleteAsync();

        // //Client streaming
        // var resp = messageClient.SendMessage();
        // for (int i = 0; i < 10; i++)
        // {
        //     await Task.Delay(1000);
        //     await resp.RequestStream.WriteAsync(new MessageRequest
        //     {
        //         Name = "tyfan",

        //         Message = "mesa " + i
        //     });
        // }

        // await resp.RequestStream.CompleteAsync();

        // System.Console.WriteLine((await resp.ResponseAsync).Message);

        //Server streaming
        //    var response =  messageClient.SendMessage(new MessageRequest
        //     {
        //         Name = "Merhaba",
        //         Message = "Tufan"
        //     });

        //     CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        //     while (await response.ResponseStream.MoveNext(cancellationTokenSource.Token))
        //     {
        //         System.Console.WriteLine(response.ResponseStream.Current.Message);
        //     }


        //Unary
        // MessageResponse response = await messageClient.SendMessageAsync(new MessageRequest{
        //     Name ="Merhaba",
        //     Message ="Tufan"
        // });

        //System.Console.WriteLine(response.Message);

        //     var greatClient = new Greeter.GreeterClient(channel);
        //    HelloReply resut =  await greatClient.SayHelloAsync(new HelloRequest
        //     {
        //         Name = "Tufandan selamlar "
        //     });

        //     Console.WriteLine(resut.Message);
    }
}