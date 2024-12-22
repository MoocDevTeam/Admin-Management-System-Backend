namespace Mooc.Model.Entity
{
    public class MoocMeida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; } 

        [ForeignKey("User")] 
        public int UploaderId { get; set; }

        [ForeignKey("Session")] 
        public int SessionID { get; set; }

        [Required]
        public FileTypeEnum FileType { get; set; } = FileTypeEnum.Video;

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } 

        [Required]
        [Column(TypeName = "text")]
        public string FilePath { get; set; } 

        [Column(TypeName = "text")]
        public string ThumbnailPath { get; set; } 

        [Required]
        public DateTime UploadedAt { get; set; } 

        [Required]
        public ApprovalStatusEnum ApprovalStatus { get; set; } = ApprovalStatusEnum.Pending;
    }

    public enum FileTypeEnum
    {
        Video,
        Audio
    }

    public enum ApprovalStatusEnum
    {
        Pending,
        Approved,
        Rejected
    }
}