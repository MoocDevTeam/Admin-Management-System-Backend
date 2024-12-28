namespace Mooc.Application.Contracts.Course;

public class EnrollmentDto:BaseEntityDto
{
    [ForeignKey("CourseInstance")]
    public long CourseInstanceId { get; set; }

    public EnrollmentStatus EnrollmentStatus { get; set; }
    public DateTime EnrollStartDate { get; set; }
    public DateTime EnrollEndDate { get; set; }
    public int MaxStudents { get; set; }

    public long CreatedByUserId { get; set; }
    public long UpdatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
