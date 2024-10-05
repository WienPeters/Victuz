using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WPCasusVictuz.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int? AddedByMemberId { get; set; } // Foreign Key for Member
        [ForeignKey(nameof(AddedByMemberId))]
        public Member? AddedByMember { get; set; }  // Navigation property for Member

        public int? AddedByBoardMemberId { get; set; } // Foreign Key for BoardMember
        [ForeignKey(nameof(AddedByBoardMemberId))]
        public BoardMember? AddedByBoardMember { get; set; }  // Navigation property for BoardMember
    }
}
