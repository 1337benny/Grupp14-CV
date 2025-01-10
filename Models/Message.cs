using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupp14_CV.Models
{
    public class Message
    {
        public Message() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageID { get; set; }

        public string SenderID { get; set; }
        
        public string ReceiverID { get; set; }

        
        [Required(ErrorMessage = "Vänligen fyll i ditt meddelande.")]
        [StringLength(500)]
        [RegularExpression(@"^[\s\S]+$")]
        public string Content { get; set; }

        public bool IsRead { get; set; } = false;


        [ForeignKey(nameof(SenderID))]
        public virtual User SendUser { get; set; }


        [ForeignKey(nameof(ReceiverID))]
        public virtual User ReceiveUser { get; set; }
    }
}
