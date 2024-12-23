using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grupp14_CV.Models
{
    public class Message
    {
        public Message() { }
        [Key]
        public int SenderID { get; set; }
        [Key]
        public int ReceiverID { get; set; }

        [Key]
        public int MessageID { get; set; }

        [Required]
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
