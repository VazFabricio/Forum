using ForumUniversitario.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ForumUniversitario.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using ForumUniversitario.Data;

namespace ForumUniversitario.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ForumUniversitarioUser> _userManager;


        public HomeController(ILogger<HomeController> logger, UserManager<ForumUniversitarioUser> userManager, ForumUniversitarioContext contexto)
        {
            _logger = logger;
            this._userManager = userManager;

        }

        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(this.User);
            var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            
            ViewData["UserName"] = user?.AccountName;
            return View();
        }

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