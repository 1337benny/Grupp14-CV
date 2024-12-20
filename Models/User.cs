using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Grupp14_CV.Models
{
    public class User : IdentityUser
    {
        public User()
        { 
            
        }
        [Required]
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool PublicSetting { get; set; }


        // public CV cvid foreign key

    }
}
