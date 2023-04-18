using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SessionAuth2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("Welcome")]
        public IActionResult Welcome()
        {
            var username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                return Unauthorized("인증되지 않은 사용자");
            }

            return Ok($"환영합니다, {username}님!");
        }
    }
}
