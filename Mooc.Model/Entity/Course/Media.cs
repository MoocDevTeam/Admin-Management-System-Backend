using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mooc.Model.Entity
{
    public class Media : BaseEntity
    {
        //Basic fields
        public MediaFileType FileType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ThumbnailPath { get; set; }
        public MediaApprovalStatus ApprovalStatus { get; set; } = MediaApprovalStatus.Pending;
        //public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //Foreign key fields
        public long SessionId { get; set; }
        public long CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }

        // Navigation user ((many-to-one))
        public virtual User CreatedByUser { get; set; }
        public virtual User UpdatedByUser { get; set; }
        public virtual Session Session { get; set; }

    }
}