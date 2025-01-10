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
                .Where(u => u.Firstname.StartsWith(query) && u.UserName != User.Identity.Name && u.UserName.Contains("@") && u.IsActive == true
                || u.Lastname.StartsWith(query) && u.UserName != User.Identity.Name && u.UserName.Contains("@") && u.IsActive == true
                )
                .Select(u => u.Firstname + " " + u.Lastname + " (" + u.UserName + ")")
                .ToList();

            return Json(new { results });
        }

        [HttpGet]
        public JsonResult SearchUser(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                // Returnera tom JSON
                return Json(new { results = new string[0] });
            }

            if (User.Identity.IsAuthenticated)
            {
                var results = users.Users
                .Where(u => u.Firstname.StartsWith(query) && u.UserName != User.Identity.Name && u.UserName.Contains("@") && u.IsActive == true
                || u.Lastname.StartsWith(query) && u.UserName != User.Identity.Name && u.UserName.Contains("@") && u.IsActive == true
                )
                .Select(u => u.Firstname + " " + u.Lastname + " (" + u.UserName + ")")
                .ToList();

                return Json(new { results });
            }
            else
            {
                var results = users.Users
                .Where(u => u.Firstname.StartsWith(query) && u.UserName != User.Identity.Name && u.PublicSetting == true && u.UserName.Contains("@") && u.IsActive == true
                || u.Lastname.StartsWith(query) && u.UserName != User.Identity.Name && u.PublicSetting == true && u.UserName.Contains("@") && u.IsActive == true
                )
                .Select(u => u.Firstname + " " + u.Lastname + " (" + u.UserName + ")")
                .ToList();

                return Json(new { results });
            }

            
        }

    }
}
