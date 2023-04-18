using Grpc.Core;
using Grpc.Net.Client;
using GrpcSessionService;

class Program
{
    static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5023");
        var client = new GrpcSession.GrpcSessionClient(channel);

        // 로그인 요청을 보냅니다.
        var loginRequest = new LoginRequest { Username = "user2", Password = "passwodrd" };
        var loginResponse = await client.LoginAsync(loginRequest);

        // 세션 ID를 가져옵니다.
        var sessionId = loginResponse.SessionId;

        // 메타데이터를 생성하고 세션 ID를 포함시킵니다.
        var headers = new Metadata
        {
            { "session-id", sessionId }
        };
        Console.WriteLine($"Session ID: {sessionId}");

        // YourMethod 호출 시 메타데이터를 전송합니다.
        var yourRequest = new YourMethodRequest();
        yourRequest.RequestData = "Hello World!";
        var yourResponse = await client.YourMethodAsync(yourRequest, headers);

        // 응답을 처리합니다.
        Console.WriteLine($"Response: {yourResponse.ResponseData}");
    }
}