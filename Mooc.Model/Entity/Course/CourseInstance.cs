namespace Mooc.Model.Entity
{
    public class CourseInstance : BaseEntity // BaseEntityWithAudit
    {
        public long MoocCourseId { get; set; }
        public int TotalSessions { get; set; }
        public CourseInstanceStatus Status { get; set; }
        public CourseInstancePermission Permission { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long CreatedByUserId { get; set; }
        public long UpdatedByUserId { get; set; }
        //public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //Navigation Properties
        public MoocCourse MoocCourse { get; set; }
        public Enrollment Enrollment { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<TeacherCourseInstance> TeacherCourseInstances { get; set; }
        public User CreatedByUser { get; set; }
        public User UpdatedByUser { get; set; }
    }
}
