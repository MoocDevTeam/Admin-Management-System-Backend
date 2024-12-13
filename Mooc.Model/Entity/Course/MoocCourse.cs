using System.ComponentModel.DataAnnotations;

namespace Mooc.Model.Entity;

public class MoocCourse : BaseEntity
{

    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(50)]
    public string CourseCode { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
    public long? CreatedByUserId { get; set; }
    public long? UpdatedByUserId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

}