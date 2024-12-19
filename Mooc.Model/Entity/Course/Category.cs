namespace Mooc.Model.Entity.Course
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public long? ParentId { get; set; }

        // foreign keys
        public long CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }

        // timestamp
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Nav user
        public virtual User CreatedByUser { get; set; }
        public virtual User UpdatedByUser { get; set; }

        //Nav Category
        public virtual Category ParentCategory { get; set; }

        // Navigation for Courses (one-to-many)
        public ICollection<MoocCourse> Courses { get; set; }


    }
}
