using Grupp14_CV.Models;
using Microsoft.AspNetCore.Mvc;

namespace Grupp14_CV.Controllers
{
    public class MessageController : Controller
    {

        private UserContext messages;

        public MessageController(UserContext service) 
        { 
            messages = service;
        }
    }
}
