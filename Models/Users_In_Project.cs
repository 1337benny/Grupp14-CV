using System.ComponentModel.DataAnnotations.Schema;

namespace Grupp14_CV.Models
{
    public class Users_In_Project
    {
        public Users_In_Project() { }

        public int ProjectID { get; set; }

        public int UserID { get; set; }

        [ForeignKey(nameof(ProjectID))]
        public virtual Project Project { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }
    }
}
