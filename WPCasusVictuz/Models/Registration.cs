using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPCasusVictuz.Models
{
    public class Registration
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        
        
        
        
        public DateTime? RegistrationDate { get; set; } = DateTime.Now;

        //public DateTime? DeRegistrationDate { get; set; } = DateTime.Now;
        public int? MemberId { get; set; }  // Foreign Key linking to Member
        // Navigation properties
        [ForeignKey(nameof(MemberId))]
        public Member? Member { get; set; }
        public int? AktivityId { get; set; }  // Foreign Key linking to Activity
        [ForeignKey(nameof(AktivityId))]
        public Aktivity? Aktivity { get; set; }
    }

}
