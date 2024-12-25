

using System;

namespace Mooc.Model.Entity;

public class User : BaseEntity
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public int Age { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Avatar { get; set; }

    public Gender Gender { get; set; }

    public DateTime? Created { get; set; }

    // MoocCourse foreign key to User
    public ICollection<MoocCourse> CreatedCourses { get; set; }
    public ICollection<MoocCourse> UpdatedCourses { get; set; }

    // Kwon: Navigation Properties for CourseInstance. They will be moved to MoocUser later
    public ICollection<CourseInstance> CreatedCourseInstances { get; set; }
    public ICollection<CourseInstance> UpdatedCourseInstances { get; set; }
}
