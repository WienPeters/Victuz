using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPCasusVictuz.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string Status { get; set; } = "active";// Example: Active, Inactive

        public int? BoardMemberId { get; set; }  // Foreign Key to BoardMember
        [ForeignKey(nameof(BoardMemberId))]
        public BoardMember? BoardMember { get; set; }  // Navigation property

        // Navigation properties
        //public ICollection<string>? Ideas { get; set; }
        public ICollection<Registration>? Registrations { get; set; }  // Many-to-many relationship with Activities
        public ICollection<Vote>? Votes { get; set; }  // One member can vote in many polls
    }

}
