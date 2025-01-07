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

		//[HttpGet]
		//public IActionResult AllMessages()
		//{
		//    var username = User.Identity.Name;
		//    var logInUser = messages.Users.FirstOrDefault(x => x.UserName == username);

		//    IQueryable<Message> messageList = from message in messages.Messages select message;

		//    messageList = messageList.Where(message => message.SenderID == logInUser.Id || message.ReceiverID == logInUser.Id);
		//    return View(messageList.Distinct().ToList());
		//}
		[HttpGet]
		public IActionResult AllMessages()
		{
			var username = User.Identity.Name;
			var logInUser = messages.Users.FirstOrDefault(x => x.UserName == username);

			if (logInUser == null)
			{
				return NotFound("Användaren hittades inte.");
			}

			var messageList = messages.Messages
				.Where(message => message.SenderID == logInUser.Id || message.ReceiverID == logInUser.Id)
				.AsEnumerable()
				.GroupBy(message =>
					new
					{
						User1 = Math.Min(message.SenderID.GetHashCode(), message.ReceiverID.GetHashCode()),
						User2 = Math.Max(message.SenderID.GetHashCode(), message.ReceiverID.GetHashCode())
					})
				.Select(group => group.First())
				.ToList();

			return View(messageList);
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

        [HttpGet]

        public IActionResult Conversation(string senderID, string recieverID)
        {
			//var username = User.Identity.Name;
			//var logInUser = messages.Users.FirstOrDefault(x => x.UserName == username);

			//if (logInUser == null)
			//{
			//	return NotFound("Användaren hittades inte.");
			//}

			var messageList = messages.Messages
				.Where(message => message.SenderID == senderID && message.ReceiverID == recieverID ||
				  message.ReceiverID == senderID && message.SenderID == recieverID
				)
				.ToList();

			return View(messageList);
        }
    }
}
