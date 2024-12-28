using Mooc.Model.Entity;

namespace Mooc.Application.Contracts.Course;

public class CategoryDto:BaseEntityDto
{
    public string CategoryName { get; set; }= string.Empty;
    public string Description { get; set; } = string.Empty;
   
    public string? IconUrl { get; set; }
    public long? ParentId { get; set; }

    public ICollection<MoocCourse> Courses { get; set; } = new List<MoocCourse>();

    public long CreatedByUserId { get; set; }
    public long UpdatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
