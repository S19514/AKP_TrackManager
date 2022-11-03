using AKP_TrackManager.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AKP_TrackManager.Models.DTO;
using Microsoft.AspNetCore.Http;

namespace AKP_TrackManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AKP_TrackManager_devContext _context;

        public HomeController(ILogger<HomeController> logger, AKP_TrackManager_devContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
         {
            var x = HttpContext.User;
            return View();
        }
        public IActionResult ReturnLoginView()
        {
            return View("Login");
        }
        //public IActionResult Login()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Login([Bind("Username,Password")] LoginCredentials credentials)
        {
            //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "edward"));
            //identity.AddClaim(new Claim(ClaimTypes.Name, "edward zhou"));
            ////add your own claims 
            //var principal = new ClaimsPrincipal(identity);
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });
            if (credentials == null)
                return View();

            var member = _context.Members.Where(m => m.Password == credentials.Password && m.EmailAddress == credentials.Username).FirstOrDefault();
            if (member != null)
            {
                var role = _context.Roles.Where(r => r.RoleId == member.RoleRoleId).FirstOrDefault();

                var identity = new ClaimsIdentity("Custom");
                var claimsIdentity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, member.Name),
                new Claim(ClaimTypes.Email, member.EmailAddress),
                new Claim(ClaimTypes.Role, role.RoleName),
                },
                "ApplicationCookie", ClaimTypes.Email, ClaimTypes.Role);
                //HttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
               //AuthenticationProperties authProperties = new AuthenticationProperties()
               //{ }
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));


                // HttpContext.User = new ClaimsPrincipal(claimsIdentity);
                var x = HttpContext.User;
                // var z = _httpContextAccessor.HttpContext.User;
                return Redirect("/Home/Index");
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            //return View("Login");
            return Redirect("/Home/Login");
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
