using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace GrpcService1.Services
{
    public class MyGrpcService : YourService.YourServiceBase
    {
        private readonly ILogger<MyGrpcService> _logger;
        public MyGrpcService(ILogger<MyGrpcService> logger)
        {
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public override async Task<YourResponse> YourMethod(YourRequest request, ServerCallContext context)
        {
            // YourRequest���� ���޵� �����͸� �����ɴϴ�.
            string requestData = request.RequestData;

            // ���� ����: ���⼭�� ��û �����͸� �빮�ڷ� �����մϴ�.
            string processedData = requestData.ToUpperInvariant();

            // YourResponse ��ü�� �����ϰ� ó���� �����͸� �����մϴ�.
            YourResponse response = new YourResponse
            {
                ResponseData = processedData
            };

            // ������ ��ȯ�մϴ�.
            return await Task.FromResult(response);

        }


        public override Task<TokenResponse> GetToken(TokenRequest request, ServerCallContext context)
        {
            // Authenticate user (use your own logic to validate the user)
            if (IsValidUser(request.Username, request.Password))
            {
                var token = GenerateJwtToken(request.Username);
                return Task.FromResult(new TokenResponse { Token = token });
            }
            else
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid username or password"));
            }
        }

        private bool IsValidUser(string username, string password)
        {
            // Implement your user authentication logic here
            return true;
        }

        private string GenerateJwtToken(string username)
        {
            // Define token expiration time
            var expiration = DateTime.UtcNow.AddHours(1);

            // Define token claims
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
        };

            // Generate token using secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here_sdhfsdf"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "your_issuer",
                audience: "your_audience",
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
