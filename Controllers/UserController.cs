using Grupp14_CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grupp14_CV.Controllers
{
    public class UserController : Controller
    {
        private UserContext users;

        public UserController(UserContext service) 
        { 
            users = service;
        }

        [HttpGet]
        public JsonResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                // Returnera tom JSON
                return Json(new { results = new string[0] });
            }

            var results = users.Users
                .Where(u => u.Firstname.StartsWith(query) || u.Lastname.StartsWith(query))
                .Select(u => u.Firstname + " " + u.Lastname)
                .ToList();

            return Json(new { results });
        }

    }
}
