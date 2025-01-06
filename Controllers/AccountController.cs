using Grupp14_CV.Models;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Diagnostics;
using System.Security.Cryptography;
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
        public IActionResult EditProfile()
        {

            var username = User.Identity.Name;
            var logInUser = users.Users.FirstOrDefault(x => x.UserName == username);

            IQueryable<User> userList = from user in users.Users select user;

            userList = userList.Where(user => user.Id == logInUser.Id);

            User theUser = userList.FirstOrDefault();

            return View(theUser);
        }

        [HttpPost]
        public IActionResult UpdateUser(string userFirstname, string userLastname, DateOnly userBirthDay, bool userPrivacy)
        {
            //Hämtar ut alla users som stämmer in på vilkoret
            IQueryable<User> userList = from user in users.Users where user.UserName == User.Identity.Name select user;

            //Sparar resultatet i ett User objekt och sätter de nya uppgifterna
            User updatedUser = userList.FirstOrDefault();
            updatedUser.Firstname = userFirstname;
            updatedUser.Lastname = userLastname;
            updatedUser.BirthDay = userBirthDay;
            updatedUser.PublicSetting = userPrivacy;

            //Sparar och uppdaterar databasen
            users.Update(updatedUser);
            users.SaveChanges();

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult CreateCV(string cvHeader, string cvContent, string cvCompetence, string cvEducation, string cvPreviousExperience)
        {
            //Skapar ett nytt CV objekt med propertys från formuläret
            CV cv = new CV();
            cv.Header = cvHeader;
            cv.Content = cvContent;
            cv.Competence = cvCompetence;
            cv.Education = cvEducation;
            cv.PreviousExperience = cvPreviousExperience;

            //Lägger till cv i databasen och sparar
            users.Add(cv);
            users.SaveChanges();

            //Hämtar ut den inloggade användaren. Sätter sedan värdet på foreign-key till det nya cv't!
            IQueryable<User> userList = from user in users.Users where user.UserName == User.Identity.Name select user;
            User updatedUser = userList.FirstOrDefault();
            updatedUser.CVID = cv.CVID;
            users.Update(updatedUser);
            users.SaveChanges();

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult UpdateCV(string cvHeader, string cvContent, string cvCompetence, string cvEducation, string cvPreviousExperience, int cvID)
        {
            //Plockar ut användarens CV
            IQueryable<CV> cvList = from cvn in users.CVs where cvn.CVID == cvID select cvn;
            CV cv = cvList.FirstOrDefault();
            
            //Sätter de nya värdena på propertys
            cv.Header = cvHeader;
            cv.Content = cvContent;
            cv.Competence = cvCompetence;
            cv.Education = cvEducation;
            cv.PreviousExperience = cvPreviousExperience;

            //Uppdaterar databasen med det uppdaterade cv't
            users.Update(cv);
            users.SaveChanges();

            

            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task <IActionResult> UpdateUserPassword(string oldPassword, string newPassword, string confirmPassword)
        {
            
            //Hämtar ut alla users som stämmer in på vilkoret
            IQueryable<User> userList = from user in users.Users where user.UserName == User.Identity.Name select user;

            //Sparar resultatet i ett User objekt 
            User updatedUser = userList.FirstOrDefault();

            //Uppdaterar användarens lösenord
            try
            {
               await userManager.ChangePasswordAsync(updatedUser, oldPassword, newPassword);
                Debug.WriteLine("Lösenordet ändrat till " + newPassword);
            }
            catch (Exception ex) {
                Debug.WriteLine("felmeddelande: " + ex.Message);
            }
           

            return RedirectToAction("Profile");
        }

        //[HttpPost]
        //public async Task<IActionResult> UpdateUserProfilePicture(string imgFile)
        //{

        //    //Hämtar ut alla users som stämmer in på vilkoret
        //    IQueryable<User> userList = from user in users.Users where user.UserName == User.Identity.Name select user;

        //    //Sparar resultatet i ett User objekt och sätter de nya uppgifterna
        //    User updatedUser = userList.FirstOrDefault();

        //    updatedUser.ProfilePicturePath = imgFile;



        //    users.Update(updatedUser);
        //    users.SaveChanges();



        //    return RedirectToAction("Profile");
        //}

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



    }
}
