using System.ComponentModel.DataAnnotations.Schema;

namespace WPCasusVictuz.Models
{
    public class Picture
    {
        public int Id { get; set; }

        //public string Name { get; set; }

        public string FilePath { get; set; }

        public int? AddedByBoardMemberId { get; set; }
        [ForeignKey(nameof(AddedByBoardMemberId))]
        public BoardMember? AddedByBoardMember { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;
    }

}
