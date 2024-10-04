using System.ComponentModel.DataAnnotations;

namespace WPCasusVictuz.Models
{
    public class Aktivity
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public int? MaxParticipants { get; set; }
        public string? Description { get; set; }

        // Navigation properties
        public ICollection<Registration>? Registrations { get; set; }  // Many-to-many relationship with Members
    }

}
