using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPCasusVictuz.Models
{
    public class Aktivity
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        public string? Category { get; set; }
        public string? Location { get; set; }
        public int? CreatedbyBM { get; set; }
        [ForeignKey(nameof(CreatedbyBM))]
        public BoardMember? MadeBy { get; set; }
        public string? Name { get; set; }
        public DateTime? Date { get; set; }

        public int? MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; } = 0;
        public string? Description { get; set; }

        // Navigation properties
        public ICollection<Registration>? Registrations { get; set; }  // Many-to-many relationship with Members
    }

}
