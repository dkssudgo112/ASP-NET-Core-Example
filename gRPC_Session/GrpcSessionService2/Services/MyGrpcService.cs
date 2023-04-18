using Grpc.Core;
using GrpcSessionService2.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace GrpcSessionService2.Services
{
    public class MyGrpcService : GrpcSession.GrpcSessionBase
    {
        private readonly ILogger<MyGrpcService> _logger;
        private readonly SessionManager _sessionManager;

        public MyGrpcService(ILogger<MyGrpcService> logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }

        // ...

        public override Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            // 사용자 인증 로직 구현 후 세션 생성
            var sessionId = _sessionManager.CreateSession(request.Username);
            return Task.FromResult(new LoginResponse { SessionId = sessionId });
        }

        public override async Task<YourMethodResponse> YourMethod(YourMethodRequest request, ServerCallContext context)
        {
            // 세션 ID를 메타데이터에서 가져옵니다.
            var sessionId = context.RequestHeaders.GetValue("session-id");
            var username = _sessionManager.GetUserFromSession(sessionId);

            if (username == null)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid session"));
            }

            //Todo : check auth
            if (username == "user")
            { 
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid session"));
            }

            // YourRequest에서 전달된 데이터를 가져옵니다.
            string requestData = request.RequestData;

            // 로직 구현: 여기서는 요청 데이터를 대문자로 변경합니다.
            string processedData = requestData.ToUpperInvariant();

            // YourResponse 객체를 생성하고 처리된 데이터를 설정합니다.
            YourMethodResponse response = new YourMethodResponse
            {
                ResponseData = processedData
            };

            // 응답을 반환합니다.
            return await Task.FromResult(response);
        }
    }

}
