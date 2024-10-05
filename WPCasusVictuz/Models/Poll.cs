using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPCasusVictuz.Models
{
    public class Poll
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        public string? Question { get; set; }
        // Store the options as a comma-separated string in the database
        public string? OptionsString { get; set; }

        

        // NotMapped: This property will be used in the application but not directly mapped to the database
        [NotMapped]
        public List<string>? Options
        {
            get => OptionsString?.Split(',').ToList();  // Convert comma-separated string to List
            set => OptionsString = value != null ? string.Join(",", value) : null;  // Convert List to comma-separated string
        }
        
        public int? CreatedByBoardMemberId { get; set; }  // FK to BoardMember
        [ForeignKey(nameof(CreatedByBoardMemberId))]
        public BoardMember? CreatedBy { get; set; }  // Navigation property to the BoardMember who created the poll

        // Navigation properties
        public ICollection<Vote>? Votes { get; set; }  // One poll can have many votes
    }

}
