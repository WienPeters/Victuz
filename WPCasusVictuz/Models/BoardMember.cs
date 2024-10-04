using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPCasusVictuz.Models
{
    public class BoardMember 
    {
        [Key]
        public int Id { get; set; }  // Primary Key for Board Member

        public string? Name { get; set; }
        public string? Password { get; set; }
        public int? MemberId { get; set; }
        [ForeignKey(nameof(MemberId))]
        public Member? Member { get; set; }
        // Navigation properties
        public ICollection<Poll>? CreatedPolls { get; set; }  // A board member can create many polls
        public ICollection<Aktivity> CreatedAktivitys { get; set; }
    }

}
