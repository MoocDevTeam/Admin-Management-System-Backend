using System.ComponentModel.DataAnnotations;

namespace Mooc.Model.Entity
{
    public class Media : BaseEntity
    {
        [Required]
        public long UploaderId { get; set; }  

        public User Uploader { get; set; }

        [Required]
        public long SessionId { get; set; }  

        public Session Session { get; set; }

        [Required]
        public FileTypeEnum FileType { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string FileName { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string FilePath { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string ThumbnailPath { get; set; }

        //[Required]
        //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

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