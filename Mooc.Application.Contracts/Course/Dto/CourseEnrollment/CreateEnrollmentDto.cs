namespace Mooc.Application.Contracts.Course;

public class CreateEnrollmentDto:BaseEntityDto
{
    [ForeignKey("CourseInstance")]
    public long CourseInstanceId { get; set; }

    public EnrollmentStatus EnrollmentStatus { get; set; }
    public DateTime EnrollStartDate { get; set; }
    public DateTime EnrollEndDate { get; set; }
    public int MaxStudents { get; set; }

    public long CreatedByUserId { get; set; } 
    public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
   
}
