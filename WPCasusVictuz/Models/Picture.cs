using System.ComponentModel.DataAnnotations.Schema;

namespace WPCasusVictuz.Models
{
    public class Picture
    {
        public int Id { get; set; }

        public string FilePath { get; set; }

        public int? AddedByMemberId { get; set; }
        [ForeignKey(nameof(AddedByMemberId))]
        public Member? AddedByMember { get; set; }

        public int? AddedByBoardMemberId { get; set; }
        [ForeignKey(nameof(AddedByBoardMemberId))]
        public BoardMember? AddedByBoardMember { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;
    }

}
