using BankOfDotNet.MvcClient.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BankOfDotNet.MvcClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // By using the Authorized attribute, we are securing the "Secure" action
        // by our authenticating authority which is IdentityServer4 which we configured
        // in the Startup.cs
        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }

        // This will handle the log-out
        public async Task Logout()
        {
            // Signout the Cookies as an approach to authenticating the user (configured in Startup.cs)
            await HttpContext.SignOutAsync("Cookies");
            // Signout the Open-ID Connect
            await HttpContext.SignOutAsync("oidc");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
