namespace Mooc.Application.Contracts.Course
{
    public class CourseInstanceDto : BaseEntityDto
    {
        public string Description { get; set; }
        public long MoocCourseId { get; set; }
        public int TotalSessions { get; set; }
        public string Status { get; set; }
        public string Permission { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CreatedByUserId { get; set; }
        public long UpdatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<TeacherReadDto> Teachers { get; set; }
        public List<ReadSessionDto> Sessions { get; set; }
        public EnrollmentDto Enrollment { get; set; }

        //user-friendly
        public string MoocCourseTitle { get; set; }
        public string CreatedUserName { get; set; }
        public string UpdatedUserName { get; set; }
    }
}
