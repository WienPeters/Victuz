using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPCasusVictuz.Models
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        
        public int? PollId { get; set; }  // Foreign Key linking to Poll
        
        public int? MemberId { get; set; }  // Foreign Key linking to Member
        public string? SelectedOption { get; set; }  // The option chosen by the member
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public bool IsSuggestion { get; set; } // True als het een suggestie is

        // Navigation properties
        [ForeignKey(nameof(PollId))]
        public Poll? Poll { get; set; }
        [ForeignKey(nameof(MemberId))]
        public Member? Member { get; set; }
    }

}
