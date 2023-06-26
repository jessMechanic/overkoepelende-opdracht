using CardGame.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Setup.Models;
using System.Diagnostics;

namespace Setup.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
     

        private const string PageViews = "PageViews";
        private readonly ILogger<HomeController> _logger;
        private UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> SignInManager,RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;
            _signInManager = SignInManager;
        }

        public IActionResult Index()
        {
           UpdatePageViewCookie();
            return View( new DataContext(){views = GetPageViewCookie()} );
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            UpdatePageViewCookie();
            return View();
            
        }

        public IActionResult Matches()
        {
            UpdatePageViewCookie();
            if (_signInManager.IsSignedIn(User))
            {
                return View();
            }
            return View("NotLoggedIn");
        }
        public IActionResult Match()
        {
            UpdatePageViewCookie();
            if (_signInManager.IsSignedIn(User))
            {
                        return View();
            }
          return View("NotLoggedIn");
       
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

        public IActionResult h2nasdj761ApiCredentials()
        {
            return View("/Views/Home/maxwell.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public  IActionResult RoleManager()
        {
            var roles =  _roleManager.Roles.ToList();
            return View(roles);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return View();
        }


        #region UserRoles

        public async Task<IActionResult> UserRoles()
        {
            var users =  _userManager.Users.ToList();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (IdentityUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }
        private async Task<List<string>> GetUserRoles(IdentityUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }


        #endregion  UserRoles






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