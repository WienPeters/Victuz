using System.ComponentModel.DataAnnotations;

namespace WPCasusVictuz.Models
{
    public class BoardMember : Member
    {
        
        public int BoardMemberId { get; set; }  // Primary Key for Board Member
        public string? Role { get; set; }  // Example: President, Treasurer, etc.

        // Navigation properties
        public ICollection<Poll>? CreatedPolls { get; set; }  // A board member can create many polls
    }

}
