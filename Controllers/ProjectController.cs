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
                (x => new SelectListItem { Text = x.Firstname, Value = x.Id.ToString() }).ToList();
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

                // Lägg till projektet och sparar
                projects.Add(project);
                projects.SaveChanges();

                //Lägger till användaren och projektet i sambandstabellen.
                Users_In_Project user_project = new Users_In_Project();
                user_project.UserID = user.Id;
                user_project.ProjectID = project.ProjectID; //Sätter projektID till projektet som precis skapats.
                projects.Add(user_project);

                //Sparar sambandet i databasen
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

        [HttpGet]
        public IActionResult ProjectInfo(int projectID)
        {
            IQueryable<Project> projectList = from project in projects.Projects where project.ProjectID == projectID select project;

            return View(projectList.FirstOrDefault());
        }

        [HttpPost]
        public IActionResult JoinProject(int projectID)
        {
            if (ModelState.IsValid)
            {
                // Hämta den inloggade användarens ID
                var username = User.Identity.Name;
                var user = projects.Users.FirstOrDefault(x => x.UserName == username);


                //Lägger till användaren och projektet i sambandstabellen.
                Users_In_Project user_project = new Users_In_Project();
                user_project.UserID = user.Id;
                user_project.ProjectID = projectID; //Sätter projektID till projektet som precis skapats.
                projects.Add(user_project);

                //Sparar sambandet i databasen
                projects.SaveChanges();

                return RedirectToAction("Project", "Project");
            }
            else
            {
                // Förbered användarvalen för ViewBag vid valideringsfel
                List<SelectListItem> users = projects.Users.Select
                    (x => new SelectListItem { Text = x.Firstname, Value = x.Id.ToString() }).ToList();
                ViewBag.options = users;

                return RedirectToAction("Project", "Project");
            }
        }

        [HttpPost]
        public IActionResult LeaveProject(int projectID)
        {
            if (ModelState.IsValid)
            {
                // Hämta den inloggade användaren
                var username = User.Identity.Name;
                var user = projects.Users.FirstOrDefault(x => x.UserName == username);

                IQueryable<Users_In_Project> uipList = from uip in projects.Users_In_Projects where uip.ProjectID == projectID && uip.UserID == user.Id select uip;

                projects.Remove(uipList.FirstOrDefault());


                //Sparar borttagningen i databasen
                projects.SaveChanges();

                return RedirectToAction("Project", "Project");
            }
            else
            {
                // Förbered användarvalen för ViewBag vid valideringsfel
                List<SelectListItem> users = projects.Users.Select
                    (x => new SelectListItem { Text = x.Firstname, Value = x.Id.ToString() }).ToList();
                ViewBag.options = users;

                return RedirectToAction("Project", "Project");
            }
        }

        [HttpPost]
        public IActionResult EditProject(string pTitel, string pDescription, DateOnly pStartDate, DateOnly pEndDate, int pID)
        {
            IQueryable<Project> projectList = from project in projects.Projects where project.ProjectID == pID select project;

            Project updatedProject = projectList.FirstOrDefault();
            updatedProject.Titel = pTitel;
            updatedProject.Description = pDescription;
            updatedProject.StartDate = pStartDate;
            updatedProject.EndDate = pEndDate;
            

            projects.Update(updatedProject);
            projects.SaveChanges();

            return RedirectToAction("Project", "Project");
        }




        }
}
