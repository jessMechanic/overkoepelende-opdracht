using Microsoft.AspNetCore.Mvc;
using Setup.Models;
using System.Diagnostics;

namespace Setup.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private const string PageViews = "PageViews";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
           UpdatePageViewCookie();
            return View( new DataContext(){views = GetPageViewCookie()} );
        }

        public IActionResult Privacy()
        {
            UpdatePageViewCookie();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void UpdatePageViewCookie()
        {
            int currentCookieValue = GetPageViewCookie();

            currentCookieValue++;
            Response.Cookies.Append(PageViews, currentCookieValue.ToString());
        }

        public int GetPageViewCookie()
        {
            string? currentCookieValue = Request.Cookies[PageViews];

            if(currentCookieValue == null) return 0;

            return int.Parse(currentCookieValue);
        }
    }
}