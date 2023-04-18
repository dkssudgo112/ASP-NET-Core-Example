using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using GrpcClient;

class Program
{
    static async Task Main(string[] args)
    {




        // JWT 토큰이 있는 경우 사용하십시오.

        // gRPC 채널을 생성합니다.
        var channel = GrpcChannel.ForAddress("http://localhost:5056", new GrpcChannelOptions
        {
            Credentials = ChannelCredentials.Insecure
        });

        // 클라이언트를 생성합니다.
        var client = new YourService.YourServiceClient(channel);


        var tokenRequest = new TokenRequest { Username = "testuser", Password = "testpassword" };
        var tokenResponse = await client.GetTokenAsync(tokenRequest);
        Console.WriteLine($"Token: {tokenResponse.Token}");




        // 메타데이터에 JWT 토큰을 추가합니다.
        var headers = new Metadata
        {
            { "Authorization", $"Bearer {tokenResponse.Token}" }
        };

        // gRPC 요청을 보냅니다.
        var yourRequest = new YourRequest2();
        yourRequest.RequestData = "hihihihihi";
        var yourResponse = await client.YourMethodAsync(yourRequest, headers);


        // 응답을 처리합니다.
        Console.WriteLine($"Response: {yourResponse.ResponseData}");
    }
}