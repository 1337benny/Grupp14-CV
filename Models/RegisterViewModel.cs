using System.ComponentModel.DataAnnotations;

namespace Grupp14_CV.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv en epost.")]
        [StringLength(255)]
        public string Epost { get; set; }

        [Required(ErrorMessage = "Vänligen skriv lösenord.")]
        [DataType(DataType.Password)]
        [Compare("BekraftaLosenord")]
        public string Losenord { get; set; }


        [Required(ErrorMessage = "Vänlingen bekräfta lösenordet")]
        [DataType(DataType.Password)]
        [Display(Name = "Bekrafta losenordet")]
        public string BekraftaLosenord { get; set; }

        [Required(ErrorMessage = "Vänligen fyll i ditt förnamn.")]
        [StringLength(50)]
        public string Fornamn { get; set; }

        [Required(ErrorMessage = "Vänligen fyll i ditt efternamn.")]
        [StringLength(50)]
        public string Efternamn { get; set; }

        [Required(ErrorMessage = "Vänligen fyll i ditt födelsedatum.")]
        public DateOnly FodelseDatum { get; set; }

    }
}
