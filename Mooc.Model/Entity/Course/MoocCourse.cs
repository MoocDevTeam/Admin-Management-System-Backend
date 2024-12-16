using System.ComponentModel.DataAnnotations;

namespace Mooc.Model.Entity;

public class MoocCourse : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public string Description { get; set; } = string.Empty;
    public long? CreatedByUserId { get; set; }
    public long? UpdatedByUserId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}