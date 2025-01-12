using Mooc.Shared.Enum;

namespace Mooc.Application.Contracts.Course
{
    public class MediaDto : BaseEntityDto
    {
        public MediaFileType FileType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
        public MediaApprovalStatus ApprovalStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long SessionId { get; set; }
        public long CreatedByUserId { get; set; }
        public long UpdatedByUserId { get; set; }
    }
}
