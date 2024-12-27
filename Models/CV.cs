using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupp14_CV.Models
{
    public class CV
    {
        public CV() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CVID { get; set; }


        [Required]
        [RegularExpression(@"^[\s\S]+$")]
        [StringLength(30)]
        public string Header { get; set; }


        [RegularExpression(@"^[\s\S]+$")]
        [StringLength(800)]
        public string Content { get; set; }

        [StringLength(500)]
        [RegularExpression(@"^[\s\S]+$")]
        public string Competence { get; set; }


        [RegularExpression(@"^[\s\S]+$")]
        [StringLength(300)]
        public string Education { get; set; }


        [RegularExpression(@"^[\s\S]+$")]
        [StringLength(400)]
        public string PreviousExperience { get; set; }

        public User Users { get; set; }

        //public virtual ICollection<User> Users { get; set; } = new List<User>();



    }
}
