using System.ComponentModel.DataAnnotations;

namespace Mooc.Model.Entity;

public class MoocCourse : BaseEntityWithAudit
{
    public string Title { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public string Description { get; set; } = string.Empty;

    // Nav User class later
    public virtual User CreatedByUser { get; set; }
    public virtual User UpdatedByUser { get; set; }
    public long CategoryId { get; set; }
    public virtual Category Category { get; set; } // Navigation property

    // Nav to a collection of CourseInstance
    public virtual ICollection<CourseInstance> CourseInstances { get; set; }

}