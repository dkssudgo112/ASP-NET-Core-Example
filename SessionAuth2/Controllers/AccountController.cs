using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SessionAuth2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpPost("Login")]
        public IActionResult Login(string username, string password)
        {
            // 사용자 인증 확인 (예: 데이터베이스에서 사용자 정보 확인)
            if (IsValidUser(username, password))
            {
                HttpContext.Session.SetString("username", username); // 세션에 사용자 이름 저장
                return Ok("로그인 성공");
            }

            return Unauthorized("로그인 실패");
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username"); // 세션에서 사용자 이름 제거
            return Ok("로그아웃 성공");
        }

        private bool IsValidUser(string username, string password)
        {
            // 실제 인증 로직 구현
            return username == "user" && password == "password";
        }
    }
}
