using System;

namespace Mooc.Model.Entity;

public class Comment : BaseEntity
{
    public long CourseId { get; set; }

    public long CreatedByUserId { get; set; }

    public string Content { get; set; }

    public bool IsActive { get; set; }

    public bool IsFlagged { get; set; }

    public long? ParentCommentId { get; set; }
}
