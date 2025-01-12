using System;

namespace Mooc.Model.Entity;

public class Comment : BaseEntityWithAudit
{
    /// <summary>
    /// The ID of the course this comment is associated with.
    /// </summary>
    public long CourseId { get; set; }

    /// <summary>
    /// The ID of the category this comment belongs to.
    /// Nullable if the comment is not categorized.
    /// </summary>
    public long? CategoryId { get; set; }

    /// <summary>
    /// The ID of the session this comment is associated with.
    /// </summary>
    public long SessionId { get; set; }

    /// <summary>
    /// The ID of the teacher this comment is associated with.
    /// Nullable if the comment is not related to a specific teacher.
    /// </summary>
    public long? TeacherId { get; set; }

    /// <summary>
    /// The actual content of the comment.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Indicates whether the comment is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Indicates whether the comment has been flagged (e.g., reported for review).
    /// </summary>
    public bool IsFlagged { get; set; }

    /// <summary>
    /// The ID of the parent comment.
    /// Nullable if this is a top-level comment.
    /// </summary>
    public long? ParentCommentId { get; set; }

    /// <summary>
    /// The associated course object.
    /// </summary>
    public MoocCourse Course { get; set; }

    /// <summary>
    /// The associated category object.
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// The associated session object.
    /// </summary>
    public Session Session { get; set; }

    /// <summary>
    /// The associated teacher object.
    /// </summary>
    public Teacher Teacher { get; set; }

    /// <summary>
    /// The parent comment object, representing the comment this is a reply to.
    /// </summary>
    public Comment ParentComment { get; set; }

    /// <summary>
    /// The collection of replies to this comment.
    /// </summary>
    public ICollection<Comment> Replies { get; set; }
}
