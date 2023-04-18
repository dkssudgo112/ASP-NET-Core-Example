using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        const string SessionKeyName = "_Name";
        const string SessionKeyFY = "_FY";
        const string SessionKeyDate = "_Date";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            HttpContext.Session.SetString(SessionKeyName, "Nam Wam");
            HttpContext.Session.SetInt32(SessionKeyFY, 2018);
            // Requires you add the Set extension method mentioned in the SessionExtensions static class.
            HttpContext.Session.Set<DateTime>(SessionKeyDate, DateTime.Now);

            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionKeyName);
            ViewBag.FY = HttpContext.Session.GetInt32(SessionKeyFY);
            ViewBag.Date = HttpContext.Session.Get<DateTime>(SessionKeyDate);

            ViewData["Message"] = "Session State In Asp.Net Core 6.0";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}