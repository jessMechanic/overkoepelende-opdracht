using Microsoft.AspNetCore.Identity;
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

        public IActionResult Matches()
        {
            UpdatePageViewCookie();
            return View();
        }
        public IActionResult Match()
        {
            UpdatePageViewCookie();
            return View();
        }
        public IActionResult contactpage()
        {
            UpdatePageViewCookie();
            return View();
        }

        public IActionResult UserProfile()
        {
            UpdatePageViewCookie();
            return View();
        }

        public IActionResult Login()
        {
            ViewData["Title"] = "Log in";
            return View("Areas/Identity/Pages/Account/Login.cshtml");
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

        public bool AcceptsCookies()
        {
            string? currentCookieValue = Request.Cookies["gdpr-consent-choice"];

            if (currentCookieValue == null) return false;
            if (currentCookieValue != "accept") return false;
            return true;
        }
    }
}