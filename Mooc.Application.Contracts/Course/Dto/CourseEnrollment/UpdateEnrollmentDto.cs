namespace Mooc.Application.Contracts.Course;

public class UpdateEnrollmentDto:BaseEntityDto
{
    public long CourseInstanceId { get; set; }

    public EnrollmentStatus EnrollmentStatus { get; set; }
    public DateTime EnrollStartDate { get; set; }
    public DateTime EnrollEndDate { get; set; }
    public int MaxStudents { get; set; }
  
    public long UpdatedByUserId { get; set; }
    public DateTime UpdatedAt { get; set; }=DateTime.UtcNow;
}
