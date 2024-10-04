using System.ComponentModel.DataAnnotations;

namespace WPCasusVictuz.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }  // Example: Active, Inactive

       

        // Navigation properties
        public ICollection<Registration>? Registrations { get; set; }  // Many-to-many relationship with Activities
        public ICollection<Vote>? Votes { get; set; }  // One member can vote in many polls
    }

}
