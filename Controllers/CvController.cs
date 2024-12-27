using Grupp14_CV.Models;
using Microsoft.AspNetCore.Mvc;

namespace Grupp14_CV.Controllers
{
    public class CvController : Controller
    {
        private UserContext cvs;

        public CvController(UserContext service) 
        { 
            cvs = service;
        }
    }
}
