namespace Mooc.Application.Contracts.Course
{
    public class CourseInstanceDto : BaseEntityDto
    {
        public string Description { get; set; }
        public long MoocCourseId { get; set; }
        //public string MoocCourseTitle { get; set; } //user-friendly
        public int TotalSessions { get; set; }
        public CourseInstanceStatus Status { get; set; }
        public CourseInstancePermission Permission { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CreatedByUserId { get; set; }
        public long UpdatedByUserId { get; set; }
        //public string CreatedUserName { get; set; } //user-friendly
        //public string UpdatedUserName { get; set; } //user-friendly
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<TeacherReadDto> Teachers { get; set; }
        public List<ReadSessionDto> Sessions { get; set; }
        public EnrollmentDto Enrollment { get; set; }
    }
}
