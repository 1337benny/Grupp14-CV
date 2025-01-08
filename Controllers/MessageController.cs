using Grupp14_CV.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace Grupp14_CV.Controllers
{
    public class MessageController : Controller
    {
        
        private UserContext messages;

        public MessageController( UserContext service) 
        {
            
            messages = service;
        }

        //[HttpGet]
        //public int GetMessageCount()
        //{
        //	var username = User.Identity.Name;
        //	var logInUser = messages.Users.FirstOrDefault(x => x.UserName == username);

        //	if (logInUser == null)
        //	{
        //		return 0;
        //	}

        //	var messageList = messages.Messages
        //		.Where(message => message.ReceiverID == logInUser.Id && message.IsRead == false)
        //		.ToList();

        //	int count = 0;
        //	foreach (var message in messageList)
        //	{
        //		count++;
        //	}

        //	return count;
        //}
        [HttpGet]
        public IActionResult GetMessageCount()
        {
            var username = User.Identity.Name;
            var logInUser = messages.Users.FirstOrDefault(x => x.UserName == username);

            if (logInUser == null)
            {
                Debug.WriteLine("ANvändaren hittas ej");
                return Json(new { count = 0 }); // Returnera JSON även om användaren inte hittades
            }

            var countMessage = messages.Messages
                .Count(message => message.ReceiverID == logInUser.Id && message.IsRead == false);

            Debug.WriteLine("Antalet meddelanden: " + countMessage);

            return Json(new { count = countMessage }); // Returnera JSON med räknat antal
        }



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

		[HttpPost]
		public IActionResult SendMessage(string recieverID, string content)
		{
			var username = User.Identity.Name;
			var logInUser = messages.Users.FirstOrDefault(x => x.UserName == username);

			Message message = new Message();
			message.Content = content;
			message.ReceiverID = recieverID;
			message.SenderID = logInUser.Id;

			messages.Add(message);
			messages.SaveChanges();


			return RedirectToAction("Conversation", new { senderID = logInUser.Id, recieverID = recieverID });

		}

		[HttpPost]
		public IActionResult ReadMessage(int messageID)
		{
			IQueryable<Message> messageList = from m in messages.Messages select m;
			messageList = messageList.Where(m => m.MessageID == messageID);
			Message message = messageList.FirstOrDefault();

			message.IsRead = true;

			messages.Update(message);
			messages.SaveChanges();


			return RedirectToAction("Conversation", new { senderID = message.SenderID, recieverID = message.ReceiverID });

		}

        //[HttpPost]
        //public async Task<IActionResult> SendNewMessageNoUser(RegisterViewModel registerViewModel, string recieverID, string content, string firstName, string lastName)
        //{
            
        //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        //    var random = new Random();
        //    var stringBuilder = new StringBuilder();

        //    for (int i = 0; i < 100; i++)
        //    {
        //        stringBuilder.Append(chars[random.Next(chars.Length)]);
        //    }

        //    string newUserName = stringBuilder.ToString();

        //    User newUser = new User();
        //    newUser.Firstname = firstName;
        //    newUser.Lastname = lastName;
        //    newUser.UserName = newUserName;
        //    newUser.BirthDay = DateOnly.FromDateTime(DateTime.Now);
        //    newUser.EmailConfirmed = false;

        //    var result = await userManager.CreateAsync(newUser, "12345Aa!");
        //    if (result.Succeeded)
        //    {
        //        //await signInManager.SignInAsync(newUser, isPersistent: true);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        foreach (var error in result.Errors)
        //        {
        //            if (error.Code == "DuplicateUserName")
        //            {
        //                ModelState.AddModelError("Epost", "E-posten är redan registrerad.");
        //            }
        //            else if (error.Code == "PasswordTooShort")
        //            {
        //                ModelState.AddModelError("Losenord", "Lösenordet är för kort.");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }
        //        }
        //    }



        //    Message message = new Message();
        //    message.Content = content;
        //    message.ReceiverID = recieverID;
        //    message.SenderID = newUser.Id;

        //    messages.Add(message);
        //    messages.SaveChanges();

        //    return RedirectToAction("Index", "Home");
        //    //return RedirectToAction("Conversation", new { senderID = logInUser.Id, recieverID = recieverID });

        //}

        
    }
}
