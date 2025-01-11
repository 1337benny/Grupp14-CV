using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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


        [Required(ErrorMessage = "Vänligen skriv en titel till ditt cv.")]
        [RegularExpression(@"^[\s\S]+$")]
        [StringLength(30)]
        public string Header { get; set; }

        [Required(ErrorMessage = "Vänligen skriv innehåll till ditt cv.")]
        [RegularExpression(@"^[\s\S]+$")]
        [StringLength(800)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Vänligen skriv kompetenser till ditt cv.")]
        [StringLength(500)]
        [RegularExpression(@"^[\s\S]+$")]
        public string Competence { get; set; }

        [Required(ErrorMessage = "Vänligen skriv utbildning till ditt cv.")]
        [RegularExpression(@"^[\s\S]+$")]
        [StringLength(300)]
        public string Education { get; set; }

        [Required(ErrorMessage = "Vänligen skriv tidigare erfarenhet till ditt cv.")]
        [RegularExpression(@"^[\s\S]+$")]
        [StringLength(400)]
        public string PreviousExperience { get; set; }

        [ValidateNever]
        public virtual User Users { get; set; }

        //public virtual ICollection<User> Users { get; set; } = new List<User>();



    }
}
