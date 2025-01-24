using Mooc.Model.Entity;

namespace Mooc.Application.Contracts.Course;

public class CourseDto : BaseEntityDto
{
    public string Title { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    public string Description { get; set; } = string.Empty;
    public long? CreatedByUserId { get; set; }
    public long? UpdatedByUserId { get; set; }
    public long? CategoryId { get; set; }

    public string CategoryName { get; set; }
    public virtual Category Category { get; set; }
    public List<CourseInstanceDto> CourseInstances { get; set; }
    // public TeacherCourseInstance teacherCourseInstance { get; set; }

}