using System.ComponentModel.DataAnnotations;
using Mooc.Model.Entity.Course;

namespace Mooc.Model.Entity;

public class MoocCourse : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public string Description { get; set; } = string.Empty;
    public User CreatedByUser { get; set; }
    public User UpdatedByUser { get; set; }
    public long CreatedByUserId { get; set; }
    public long UpdatedByUserId { get; set; }
    // public Category Category { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Foreign key for Category
    public long CategoryId { get; set; }
    public virtual Category Category { get; set; } // Navigation property

}