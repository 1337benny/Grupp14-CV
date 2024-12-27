using Grupp14_CV.Models;
using Microsoft.AspNetCore.Mvc;

namespace Grupp14_CV.Controllers
{
    public class ProjectController : Controller
    {
        private UserContext projects;

        public ProjectController(UserContext service)
        {
            projects = service;
        }
    }
}
