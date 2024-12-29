using Grupp14_CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Grupp14_CV.Controllers
{
    public class ProjectController : Controller
    {
        private UserContext projects;

        public ProjectController(UserContext service)
        {
            projects = service;
        }

        
        public IActionResult Project()
        {
            IQueryable<Project> projectList = from project in projects.Projects select project;
            return View(projectList.ToList());
        }
        [HttpGet]
        public IActionResult AddProject()
        {
            Project project = new Project();
            List<SelectListItem> users = projects.Users.Select
                (x => new SelectListItem { Text = x.Firstname, Value = x.Id.ToString()}).ToList();
            ViewBag.options = users;
            return View(project);
        }
        [HttpPost]
        public IActionResult AddProject(Project project)
        {
            if (ModelState.IsValid)
            {
                // Hämta den inloggade användarens ID
                var username = User.Identity.Name;
                var user = projects.Users.FirstOrDefault(x => x.UserName == username);

                if (user == null)
                {
                    ModelState.AddModelError("", "Det gick inte att hitta den inloggade användaren.");
                    return View(project);
                }

                // Tilldela CreatorID
                project.CreatorID = user.Id;

                // Lägg till projektet och spara
                projects.Add(project);
                projects.SaveChanges();
                return RedirectToAction("Project", "Project");
            }
            else
            {
                // Förbered användarvalen för ViewBag vid valideringsfel
                List<SelectListItem> users = projects.Users.Select
                    (x => new SelectListItem { Text = x.Firstname, Value = x.Id.ToString() }).ToList();
                ViewBag.options = users;

                return View(project);
            }
        }

        //[HttpPost]
        //public IActionResult AddProject(Project project)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        project.CreatorID = projects.Users.FirstOrDefault(x => x.UserName == User.Identity.Name).Id;
        //        projects.Add(project);
        //        projects.SaveChanges();
        //        return RedirectToAction("Project", "Project");
        //    } else
        //    {
        //        List<SelectListItem> users = projects.Users.Select
        //         (x => new SelectListItem { Text = x.Firstname, Value = x.Id.ToString() }).ToList();
        //        ViewBag.options = users;
        //        return View(project);
        //    }
        //}
    }
}
