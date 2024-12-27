using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupp14_CV.Models
{
    public class User : IdentityUser
    {
        public User()
        { 
            
        }

        [Required]
        [RegularExpression(@"^[\p{L}\s]+$")]
        [StringLength(50)]
        public string Firstname { get; set; }


        [Required]
        [RegularExpression(@"^[\p{L}\s]+$")]
        [StringLength(50)]
        public string Lastname { get; set; }

        
        public bool PublicSetting { get; set; } = true;

        [Required]
        public DateOnly BirthDay { get; set; }


        public int? CVID { get; set; }

        [ForeignKey(nameof(CVID))]
        public CV CV { get; set; }

        public virtual IEnumerable<Message> SentMessages { get; set; } = new List<Message>();

        public virtual IEnumerable<Message> RecievedMessages { get; set; } = new List<Message>();

        public virtual IEnumerable<Users_In_Project> UsersInProject { get; set; } = new List<Users_In_Project>();

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        //public virtual IEnumerable<User> MessagesUsers { get; set; } = new List<User>();
    }
}
