namespace Mooc.Model.Entity.Course
{
    public class MoocTeacher : BaseEntity
    {
        public string Title { get; set; }
        public string Department { get; set; }
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
    }
}
