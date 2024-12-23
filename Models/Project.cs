using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupp14_CV.Models
{
    public class Project
    {
        public Project () { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectID { get; set; }


        [Required]
        [RegularExpression(@"^[\s\S]+$")]
        [StringLength(50)]
        public string Titel {  get; set; }


        [Required]
        [RegularExpression(@"^[\s\S]+$")]
        [StringLength(300)]
        public string Description { get; set; }


        [Required]
        public DateOnly StartDate { get; set; }


        public DateTime EndDate { get; set; }


        [Required]
        public int CreatorID { get; set; }


        
        [ForeignKey(nameof(CreatorID))]
        public virtual User user { get; set; }


        public virtual IEnumerable<Users_In_Project> UsersInProject { get; set; } = new List<Users_In_Project>();
    }
}
