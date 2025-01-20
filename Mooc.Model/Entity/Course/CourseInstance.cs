namespace Mooc.Model.Entity
{
    public class CourseInstance : BaseEntityWithAudit
    {
        public string Description { get; set; }
        public long MoocCourseId { get; set; }
        public CourseInstanceStatus Status { get; set; }
        public CourseInstancePermission Permission { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //Navigation Properties
        public MoocCourse MoocCourse { get; set; }
        public Enrollment Enrollment { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<TeacherCourseInstance> TeacherCourseInstances { get; set; }
        public User CreatedByUser { get; set; }
        public User UpdatedByUser { get; set; }
    }
}
