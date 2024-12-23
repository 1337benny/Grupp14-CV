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


        public int CVID { get; set; }

        [ForeignKey(nameof(CVID))]
        public virtual CV CV { get; set; }



    }
}
