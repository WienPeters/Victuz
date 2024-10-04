using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPCasusVictuz.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        
        public int? MemberId { get; set; }  // Foreign Key linking to Member
        
        public int? AktivityId { get; set; }  // Foreign Key linking to Activity
        public DateTime? RegistrationDate { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey(nameof(MemberId))]
        public Member? Member { get; set; }
        [ForeignKey(nameof(AktivityId))]
        public Aktivity? Aktivity { get; set; }
    }

}
