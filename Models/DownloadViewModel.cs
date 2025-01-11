namespace Grupp14_CV.Models
{
    [Serializable]
    public class DownloadViewModel
    {
        //public User User { get; set; }
        //public List<Project> Projects { get; set; }
        //public CV CV { get; set; }

        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string UserBirthDay { get; set; }
        public bool UserIsActive { get; set; }
        public bool UserPublicSetting { get; set; }

        public string CvHeader { get; set; }
        public string CvDescription { get; set; }
        public string CvCompetence { get; set; }
        public string CvEducation { get; set; }
        public string CvPreviousExperience { get; set; }

        public List<string> projects { get; set;} = new List<string>();


    }
}
