using Microsoft.EntityFrameworkCore;

namespace Grupp14_CV.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Users_In_Project> Users_In_Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CV>().HasData(
                new CV
                {
                    CVID = 1,
                    Header = "Johans CV",
                    Content = "Hej! Jag heter Johan och är född 2001. Jag gillar mysiga hemmakvällar och att programmera.",
                    Competence = "Lösningsorienterad, tålmodig, klok.",
                    Education = "Europaskolan Gymnasie & Örebro Universitet Systemvetenskap",
                    PreviousExperience = "Ica, Willys, Netbeans, Rewatch, SQL-Server, HTML, Alla språk."
                },
                new CV
                {
                    CVID = 2,
                    Header = "Antons CV",
                    Content = "Hej! Jag heter Anton och är född 1998. Jag gillar sport, teknik och att laga mat.",
                    Competence = "Noggrann, initiativrik, samarbetsvillig.",
                    Education = "Stockholms Universitet Datavetenskap",
                    PreviousExperience = "Coop, Elgiganten, C#, React, Node.js, MongoDB."
                },
                new CV
                {
                    CVID = 3,
                    Header = "Williams CV",
                    Content = "Hej! Jag heter William och är född 1995. Jag älskar att leda team, skapa lösningar och resa.",
                    Competence = "Ledarskapsförmåga, kreativ, kommunikativ.",
                    Education = "Chalmers Tekniska Högskola Industriell Ekonomi",
                    PreviousExperience = "Rewatch, Microsoft, Azure, Python, SQL, Vue.js."
                },
                new CV
                {
                    CVID = 4,
                    Header = "Oskars CV",
                    Content = "Hej! Jag heter Oskar och är född 1997. Jag trivs med att arbeta med detaljer och analysera problem.",
                    Competence = "Analytisk, strukturerad, lösningsorienterad.",
                    Education = "Lunds Universitet Civilingenjör Informationsteknik",
                    PreviousExperience = "Hemköp, Rewatch, Java, Docker, Kubernetes, MySQL, PHP."
                },
                new CV
                {
                    CVID = 5,
                    Header = "Olivers CV",
                    Content = "Hej! Jag heter Oliver och är född 2000. Jag brinner för ny teknik och att bygga intuitiva användarupplevelser.",
                    Competence = "Innovativ, användarcentrerad, flexibel.",
                    Education = "KTH Kungliga Tekniska Högskolan Datateknik",
                    PreviousExperience = "NetOnNet, MediaMarkt, Kotlin, Swift, Figma, UX/UI Design."
                }


                );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Firstname = "Johan",
                    Lastname = "Rosenkvist",
                    CVID = 1,
                    PasswordHash = "losenord",
                    Email = "johan@example.com",
                    BirthDay = new DateOnly(2001, 03, 14)
                },
                new User
                {
                    Firstname = "Anton",
                    Lastname = "Kraft",
                    CVID = 2,
                    PasswordHash = "losenord",
                    Email = "anton@example.com",
                    BirthDay = new DateOnly(1995, 04, 02)
                },
                new User
                {
                    Firstname = "William",
                    Lastname = "Lagerqvist",
                    CVID = 3,
                    PasswordHash = "losenord",
                    Email = "william@example.com",
                    BirthDay = new DateOnly(2000, 06, 06)
                },
                new User
                {
                    Firstname = "Oskar",
                    Lastname = "Prenkert",
                    CVID = 4,
                    PasswordHash = "losenord",
                    Email = "oskar@example.com",
                    BirthDay = new DateOnly(1999, 07, 15)
                },
                new User
                {
                    Firstname = "Oliver",
                    Lastname = "Edvinson",
                    CVID = 5,
                    PasswordHash = "losenord",
                    Email = "oliver@example.com",
                    BirthDay = new DateOnly(2001, 06, 10)
                }

                );


            
            modelBuilder.Entity<Message>().HasData(
                new Message
                {
                    SenderID = 1,
                    ReceiverID = 2,
                    MessageID = 1,
                    Content = "Hej, vgd?"
                }
                );

            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    ProjectID = 1,
                    Titel = "Ny hemisda till Elgiganten",
                    Description = "HTML, CSS, JS, SQL. Fullstack development för kunden Elgiganten.",
                    StartDate = new DateOnly(2024, 10, 10),
                    CreatorID = 1,
                }
                );
            modelBuilder.Entity<Users_In_Project>().HasData(
                new Users_In_Project
                {
                    ProjectID = 1,
                    UserID = 1
                },
                new Users_In_Project
                {
                    ProjectID = 1,
                    UserID = 2
                },
                new Users_In_Project
                {
                    ProjectID = 1,
                    UserID = 3
                },
                new Users_In_Project
                {
                    ProjectID = 1,
                    UserID = 4
                },
                new Users_In_Project
                {
                    ProjectID = 1,
                    UserID = 5
                }

                );
        }
    }
}
