namespace Mooc.Model.Entity.Course
{
    public class Teacher : BaseEntity
    {
        public string Title { get; set; }
        public string Department { get; set; }

        //To show if this teacher is still hired or has course assigned
        public bool IsActive { get; set; }
        public string Introduction { get; set; }
        public string Expertise { get; set; }
        public string Office { get; set; }
        public DateTime HiredDate { get; set; }

        // foreign keys
        public long CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }

        // timestamp
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Nav use MoocUser class later
        public virtual User CreatedByUser { get; set; }
        public virtual User UpdatedByUser { get; set; }

        // Nav to a collction of CourseInstance assigned to this teacher
        //Need to modify when MoocCourseInstance is renamed!!!
        // - Collection Initializer: Ensures the property is not null by default and ready for use.
        public virtual ICollection<MoocCourseInstance> AssignedCourses { get; set; } = new List<MoocCourseInstance>();
    }
}
