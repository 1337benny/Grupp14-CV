using Grupp14_CV.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Grupp14_CV.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManag, SignInManager<User> signInMang)
        {
            this.userManager = userManag;
            this.signInManager = signInMang;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User anvandare = new User();
        //        anvandare.Email = registerViewModel.Epost;
        //        anvandare.Firstname = registerViewModel.Fornamn;
        //        anvandare.Lastname = registerViewModel.Efternamn;
        //        anvandare.BirthDay = registerViewModel.FodelseDatum;

        //        var result =
        //        await userManager.CreateAsync(anvandare, registerViewModel.Losenord);
        //        if (result.Succeeded)
        //        {
        //            await signInManager.SignInAsync(anvandare, isPersistent: true);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            foreach (var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }
        //        }
        //    }
        //    return View(registerViewModel);
        //}

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
    }
}
