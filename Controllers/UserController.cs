using Grupp14_CV.Models;
using Microsoft.AspNetCore.Mvc;

namespace Grupp14_CV.Controllers
{
    public class UserController : Controller
    {
        private UserContext users;

        public UserController(UserContext service) 
        { 
            users = service;
        }
    }
}
