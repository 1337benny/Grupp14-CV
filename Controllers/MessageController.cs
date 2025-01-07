using Grupp14_CV.Models;
using Microsoft.AspNet.Identity;
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

        [HttpGet]
        public IActionResult AllMessages()
        {

            return View();
        }

        [HttpGet]
        public IActionResult NewMessage()
        {

            return View();
        }

        [HttpPost]
        public IActionResult SendNewMessage(string search, string content)
        {
            //Hämtar ut mottagarens email
            string email = search.Split('(', ')')[1];

            //Hämtar ut mottagaren i ett User objekt 
            IQueryable<User> userList = from user in messages.Users select user;
            userList = userList.Where(user => user.UserName == email);
            User theUser = userList.FirstOrDefault();

            //Hämtar ut den inloggades id
            var username = User.Identity.Name;
            var logInUser = messages.Users.FirstOrDefault(x => x.UserName == username);
            IQueryable<User> senderList = from user in messages.Users select user;
            senderList = senderList.Where(user => user.Id == logInUser.Id);
            User sender = senderList.FirstOrDefault();

            Message message = new Message();
            message.Content = content;
            message.ReceiverID = theUser.Id;
            message.SenderID = sender.Id;

            messages.Add(message);
            messages.SaveChanges();

            return RedirectToAction("AllMessages", "Message");
        }
    }
}
