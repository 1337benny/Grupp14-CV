using Grupp14_CV.Models;
using Microsoft.AspNetCore.Mvc;

namespace Grupp14_CV.Controllers
{
    public class Users_In_ProjectsController : Controller
    {
        private UserContext usersInProject;

        public Users_In_ProjectsController(UserContext service)
        {
            usersInProject = service;
        }
    }
}
