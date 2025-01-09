using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupp14_CV.Models
{
    public class User : IdentityUser
    {
        public User()
        { 
            
        }

        [Required(ErrorMessage = "Vänligen lämna inte förnamn tomt.")]
        [RegularExpression(@"^[\p{L}\s]+$")]
        [StringLength(50)]
        public string Firstname { get; set; }


        [Required(ErrorMessage = "Vänligen lämna inte efternamn tomt.")]
        [RegularExpression(@"^[\p{L}\s]+$")]
        [StringLength(50)]
        public string Lastname { get; set; }

        
        public bool PublicSetting { get; set; } = true;

        [Required(ErrorMessage = "Vänligen lämna inte datum tomt.")]
        public DateOnly BirthDay { get; set; }

        public string? ProfilePicturePath { get; set; } = "/images/4a7de5d7-70f5-4a2a-9ef1-f691e98a4c38.jpeg";


        public int? CVID { get; set; }

        [ForeignKey(nameof(CVID))]
        [ValidateNever]
        public virtual CV CV { get; set; }

        public virtual IEnumerable<Message> SentMessages { get; set; } = new List<Message>();

        public virtual IEnumerable<Message> RecievedMessages { get; set; } = new List<Message>();

        public virtual IEnumerable<Users_In_Project> UsersInProject { get; set; } = new List<Users_In_Project>();

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        //public virtual IEnumerable<User> MessagesUsers { get; set; } = new List<User>();
    }
}
