
namespace Mooc.Model.Entity
{
    public class Enrollment : BaseEntity
    {
        [ForeignKey("CourseInstance")]
        public long CourseInstanceId { get; set; }

        public EnrollmentStatus EnrollmentStatus { get; set; }
        public DateTime EnrollStartDate { get; set; }
        public DateTime EnrollEndDate { get; set; }
        public int MaxStudents { get; set; }

        [ForeignKey("CreatedByUser")]
        public long CreatedByUserId { get; set; }
        [ForeignKey("UpdatedByUser")]
        public long UpdatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
