using Grupp14_CV.Models;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.IO;
//using PasswordVerificationResult = Microsoft.AspNet.Identity.PasswordVerificationResult;

namespace Grupp14_CV.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        private UserContext users;

        private string logInUserID;

        public AccountController(UserManager<User> userManag, SignInManager<User> signInMang, UserContext service)
        {
            this.userManager = userManag;
            this.signInManager = signInMang;
            this.users = service;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                User anvandare = new User
                {
                    Email = registerViewModel.Epost,
                    Firstname = registerViewModel.Fornamn,
                    Lastname = registerViewModel.Efternamn,
                    BirthDay = registerViewModel.FodelseDatum,
                    UserName = registerViewModel.Epost,
                    EmailConfirmed = false
                };

                var result = await userManager.CreateAsync(anvandare, registerViewModel.Losenord);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(anvandare, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError("Epost", "E-posten är redan registrerad.");
                        }
                        else if (error.Code == "PasswordTooShort")
                        {
                            ModelState.AddModelError("Losenord", "Lösenordet är för kort.");
                        }
                        else
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            return View(registerViewModel);
        }


        [HttpGet]
        public IActionResult LogIn()
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                loginViewModel.Epost,
                loginViewModel.Losenord,
                isPersistent: loginViewModel.RememberMe,
                lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Fel användarnam/lösenord.");
                }
            }
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Profile()
        {

            var username = User.Identity.Name;
            var logInUser = users.Users.FirstOrDefault(x => x.UserName == username);

            IQueryable<User> userList = from user in users.Users select user;

            userList = userList.Where(user => user.Id == logInUser.Id);

            User theUser = userList.FirstOrDefault();

            logInUserID = theUser.Id;
            
            return View(theUser);
        }

        [HttpGet]
        public IActionResult OtherProfile(string searchUser)
        {

            string email = searchUser.Split('(', ')')[1];

            IQueryable<User> userList = from user in users.Users select user;

            userList = userList.Where(user => user.UserName == email);

            User theUser = userList.FirstOrDefault();

            logInUserID = theUser.Id;

            ProfileVisitor(theUser);

            return View(theUser);
        }

        [HttpPost]
        public void ProfileVisitor(User user)
        {
            user.ProfileVisitors = user.ProfileVisitors + 1;
            users.Update(user);
            users.SaveChanges();
            
        }

        [HttpGet]
        public IActionResult EditProfile()
        {

            var username = User.Identity.Name;
            var logInUser = users.Users.FirstOrDefault(x => x.UserName == username);

            IQueryable<User> userList = from user in users.Users select user;

            userList = userList.Where(user => user.Id == logInUser.Id);

            User theUser = userList.FirstOrDefault();


            EditProfileViewModel viewModel = new EditProfileViewModel();
            viewModel.User = theUser;
            viewModel.CV = theUser.CV;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateUser(User newUser, string lastname, string firstname, DateOnly birthday, bool privacy)
        {
            //Hämtar ut alla users som stämmer in på vilkoret
            IQueryable<User> userList = from user in users.Users where user.UserName == User.Identity.Name select user;

            //Sparar resultatet i ett User objekt och sätter de nya uppgifterna
            User updatedUser = userList.FirstOrDefault();

            if (updatedUser == null)
            {
                return NotFound(); // Hantera om användaren inte hittas
            }

            // Ta bort validering för vissa fält om det behövs
            ModelState.Remove("CV");

            if (ModelState.IsValid)
            {

                updatedUser.Firstname = newUser.Firstname;
                updatedUser.Lastname = newUser.Lastname;
                updatedUser.BirthDay = newUser.BirthDay;
                updatedUser.PublicSetting = privacy;


                //Sparar och uppdaterar databasen
                users.Update(updatedUser);
                users.SaveChanges();

                return RedirectToAction("Profile");
            }
            else
            {
                EditProfileViewModel viewModel = new EditProfileViewModel
                {
                    User = updatedUser,
                    CV = updatedUser.CV
                };

                return View("EditProfile", viewModel);
            }
        }

        [HttpPost]
        public IActionResult CreateCV(CV cv)
        {
            IQueryable<User> userList = from user in users.Users where user.UserName == User.Identity.Name select user;
            User updatedUser = userList.FirstOrDefault();
            if (ModelState.IsValid)
            {

                //Lägger till cv i databasen och sparar
                users.Add(cv);
                users.SaveChanges();

                //Hämtar ut den inloggade användaren. Sätter sedan värdet på foreign-key till det nya cv't!
               
                updatedUser.CVID = cv.CVID;
                users.Update(updatedUser);
                users.SaveChanges();

                return RedirectToAction("Profile");
            }
            else
            {
                // Om validering misslyckas, skicka tillbaka vyn med data
                EditProfileViewModel viewModel = new EditProfileViewModel
                {
                    User = updatedUser,
                    CV = cv
                };

                return View("EditProfile", viewModel);
            }
        }

        [HttpPost]
        public IActionResult UpdateCV(CV cv)
        {
            // Hämta aktuell användare
           IQueryable<User> list = from user in users.Users where user.UserName == User.Identity.Name select user;
            User logUser = list.FirstOrDefault();

            // Sätt Users innan validering
            cv.Users = logUser;

            // Validera manuellt
           ModelState.Remove("Users"); // Ta bort Users från den initiala ModelState-valideringen

            if (ModelState.IsValid)
            {
                // Plocka ut användarens CV
                CV cvDb = users.CVs.FirstOrDefault(c => c.CVID == cv.CVID);

                if (cvDb != null)
                {
                    // Uppdatera CV-egenskaper
                    cvDb.Header = cv.Header;
                    cvDb.Content = cv.Content;
                    cvDb.Competence = cv.Competence;
                    cvDb.Education = cv.Education;
                    cvDb.PreviousExperience = cv.PreviousExperience;

                    // Sätt användare
                    cvDb.Users = cv.Users;

                    // Uppdatera databasen
                    users.Update(cvDb);
                    users.SaveChanges();
                }

                return RedirectToAction("Profile");
            }
            else
            {
                // Om validering misslyckas, skicka tillbaka vyn med data
                EditProfileViewModel viewModel = new EditProfileViewModel
                {
                    User = logUser,
                    CV = cv
                };

                return View("EditProfile", viewModel);
            }
        }


        [HttpPost]
        public async Task <IActionResult> UpdateUserPassword(string oldPassword, string newPassword, string confirmPassword)
        {
            IQueryable<User> list = from user in users.Users where user.UserName == User.Identity.Name select user;
            User logUser = list.FirstOrDefault();

            EditProfileViewModel viewModel = new EditProfileViewModel
            {
                User = logUser,
                CV = logUser.CV
            };

            //Kollar om alla fält är ifyllda
            if (ModelState.IsValid)
            {
                //Om kontrollen av det nya lösenordet inte stämmer, avbryt.
                if (!newPassword.Equals(confirmPassword))
                {
                    ModelState.AddModelError("Losenord", "Det nya lösenordet matchar inte.");
                    return View("EditProfile", viewModel);
                }

                //Försök att ändra lösenord
                var result = await userManager.ChangePasswordAsync(logUser, oldPassword, newPassword);
                

                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
                
                else
                {
                    foreach (var error in result.Errors)
                    {
                        //Om error är att det gamla lösenordet inte stämmer med databasen
                        if (error.Code == "PasswordMismatch")
                        {
                            ModelState.AddModelError("Losenord", "Det gamla lösenordet är inte korrekt. Lösenordet har inte ändrats.");
                            return View("EditProfile", viewModel);
                        }

                    }
                    //Skriv ut "regex" för lösenordets krav.
                    ModelState.AddModelError("", "Lösenordet måste innehålla:");
                    ModelState.AddModelError("", "Minst 6 tecken långt");
                    ModelState.AddModelError("", "Bokstav, stor och liten");
                    ModelState.AddModelError("", "Tecken (ex: !%&=?)");
                    return View("EditProfile", viewModel);

                }

                
            }
            else
            {
                //Skickar tillbaka vilka fält som glömts
                return View("EditProfile", viewModel);
            }

            
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserProfilePicture(IFormFile imgFile)
        {
            if (imgFile != null && imgFile.Length > 0)
            {
                // Kontrollera att filen är en bild
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(imgFile.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("ProfilePicture", "Endast bildfiler (JPG, PNG, GIF) är tillåtna.");
                    return RedirectToAction("Profile");
                }

                // Hämta användaren från databasen
                IQueryable<User> userList = from user in users.Users where user.UserName == User.Identity.Name select user;
                User updatedUser = userList.FirstOrDefault();
                if (updatedUser == null)
                {
                    return NotFound("Användaren hittades inte.");
                }

                // Generera ett unikt filnamn
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

                // Sätt sökvägen där bilden sparas
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                var filePath = Path.Combine(uploadPath, uniqueFileName);

                // Spara filen
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imgFile.CopyToAsync(stream);
                }

                // Uppdatera användarens profilbildväg
                updatedUser.ProfilePicturePath = $"/images/{uniqueFileName}";
                users.Update(updatedUser);
                users.SaveChanges();
            }
            else
            {
                ModelState.AddModelError("ProfilePicture", "Ingen bild valdes.");
            }

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> SendNewMessageNoUser(/*RegisterViewModel registerViewModel,*/ string search, string content, string firstName, string lastName)
        {
            if (ModelState.IsValid)
            {
                string email = search.Split('(', ')')[1];

                //Hämtar ut mottagaren i ett User objekt 
                IQueryable<User> userList = from user in users.Users select user;
                userList = userList.Where(user => user.UserName == email);
                User theUser = userList.FirstOrDefault();

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var random = new Random();
                var stringBuilder = new StringBuilder();

                for (int i = 0; i < 100; i++)
                {
                    stringBuilder.Append(chars[random.Next(chars.Length)]);
                }

                string newUserName = stringBuilder.ToString();

                User newUser = new User();
                newUser.Firstname = firstName;
                newUser.Lastname = lastName;
                newUser.UserName = newUserName;
                newUser.BirthDay = DateOnly.FromDateTime(DateTime.Now);
                newUser.EmailConfirmed = false;

                var result = await userManager.CreateAsync(newUser, "12345Aa!");
                if (result.Succeeded)
                {
                    Message message = new Message();
                    message.Content = content;
                    message.ReceiverID = theUser.Id;
                    message.SenderID = newUser.Id;

                    users.Add(message);
                    users.SaveChanges();
                    //await signInManager.SignInAsync(newUser, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError("Epost", "E-posten är redan registrerad.");
                        }
                        else if (error.Code == "PasswordTooShort")
                        {
                            ModelState.AddModelError("Losenord", "Lösenordet är för kort.");
                        }
                        else
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                return RedirectToAction("Index", "Home");
                //return RedirectToAction("Conversation", new { senderID = logInUser.Id, recieverID = recieverID });
            }
            else
            {
                ModelState.AddModelError("Losenord", "Vänligen fyll i mottagare.");
                return RedirectToAction("NewMessage", "Message");
            }
        }

        [HttpPost]
        public IActionResult UpdateUserActive(bool active)
        {
            IQueryable<User> userList = from user in users.Users select user;
            userList = userList.Where(user => user.UserName == User.Identity.Name);
            User theUser = userList.FirstOrDefault();

            if (active)
            {
                theUser.IsActive = true;
            }
            else
            {
                theUser.IsActive = false;
            }

            users.Update(theUser);
            users.SaveChanges();

            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult FindMatchingProfiles(int searchAge)
        {
            var randomCVList = users.Users
               .Where(user => user.BirthDay.Year > searchAge - 5 && user.BirthDay.Year < searchAge + 5 && user.IsActive == true && user.CV != null)
               .OrderBy(c => Guid.NewGuid()) // Slumpa ordningen med hjälp av Guid.NewGuid
               .Take(3) // Hämta de första 3
               .ToList();

            return View(randomCVList);
        }

        [HttpGet]
        public IActionResult DownloadProfile()
        {
            //Hämtar ut den inloggade användaren
            IQueryable<User> userList = from user in users.Users select user;
            userList = userList.Where(user => user.UserName == User.Identity.Name);
            User downloadUser = userList.FirstOrDefault();

            //Skapar ett nytt cv
            CV downloadCV = new CV();
            downloadCV.Header = "Saknas";
            downloadCV.PreviousExperience = "Saknas";
            downloadCV.Content = "Saknas";
            downloadCV.Competence = "Saknas";
            downloadCV.Education = "Saknas";

            //Om personen har ett cv så använder vi det.
            if (downloadUser.CV != null)
            {
               downloadCV = downloadUser.CV;
            }
            //Lägger till information i alla properties. 
            DownloadViewModel downloadViewModel = new DownloadViewModel();
            downloadViewModel.UserFirstName = downloadUser.Firstname;
            downloadViewModel.UserLastName = downloadUser.Lastname;
            downloadViewModel.UserEmail = downloadUser.Email;
            downloadViewModel.UserBirthDay = downloadUser.BirthDay.ToString();
            downloadViewModel.UserIsActive = downloadUser.IsActive;
            downloadViewModel.UserPublicSetting = downloadUser.PublicSetting;
            downloadViewModel.CvHeader = downloadCV.Header;
            downloadViewModel.CvDescription = downloadCV.Content;
            downloadViewModel.CvCompetence = downloadCV.Competence;
            downloadViewModel.CvEducation = downloadCV.Education;
            downloadViewModel.CvPreviousExperience = downloadCV.PreviousExperience;
            foreach (Users_In_Project uip in downloadUser.UsersInProject)
            {
                downloadViewModel.projects.Add(uip.Project.Titel);
                downloadViewModel.projects.Add(uip.Project.Description);
                downloadViewModel.projects.Add(uip.Project.StartDate.ToString());
                downloadViewModel.projects.Add(uip.Project.EndDate.ToString());

            }
            //Hämtar användarens skrivbordsväg och skapar filvägen till xml-filen.
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "DownloadViewModel.xml");

            //Skapar serialiseraren
            XmlSerializer serializer = new XmlSerializer(typeof(DownloadViewModel));

            //Serialiserar objektet!
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, downloadViewModel);
            }

            //Skickar till html att nedladdningen är klar. Så att vi kan skriva ut ett meddelande till användaren
            ViewBag.DownloadSuccess = true;

            return View("Profile", downloadUser);

        }
    }
}
