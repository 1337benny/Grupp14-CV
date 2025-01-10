using System.Diagnostics;
using Grupp14_CV.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Grupp14_CV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserContext users;

        public HomeController(ILogger<HomeController> logger, UserContext service)
        {
            _logger = logger;
            users = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var randomCVList = users.CVs
                    /*.Where(user => user.Users.IsActive == true*/ /*&& user.Users.UserName != User.Identity.Name*/
                .OrderBy(c => Guid.NewGuid()) // Slumpa ordningen med hjälp av Guid.NewGuid
                .Take(5) // Hämta de första 5
                .ToList();
                return View(randomCVList);
            }
            else
            {
                var randomCVList = users.CVs
                //.Where(cv => cv.Users.PublicSetting == true && cv.Users.IsActive == true)
                .OrderBy(c => Guid.NewGuid()) // Slumpa ordningen med hjälp av Guid.NewGuid
                .Take(5) // Hämta de första 5
                .ToList();

                return View(randomCVList);
            }
            
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
