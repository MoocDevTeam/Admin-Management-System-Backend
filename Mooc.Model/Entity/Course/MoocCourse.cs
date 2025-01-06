using System.ComponentModel.DataAnnotations;

namespace Mooc.Model.Entity;

public class MoocCourse : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public string Description { get; set; } = string.Empty;
    public virtual User CreatedByUser { get; set; }
    public virtual User UpdatedByUser { get; set; }
    public long CreatedByUserId { get; set; }
    public long UpdatedByUserId { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long CategoryId { get; set; }
    public virtual Category Category { get; set; } // Navigation property
    public virtual ICollection<CourseInstance> CourseInstances { get; set; }

}