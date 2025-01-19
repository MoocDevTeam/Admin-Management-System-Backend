namespace Mooc.Model.Entity
{
    public class Session : BaseEntityWithAudit
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }

        //Foreign keys
        public long CourseInstanceId { get; set;}

        // Navigation user 
        public virtual User CreatedByUser { get; set; }
        public virtual User UpdatedByUser { get; set; }
        public virtual CourseInstance CourseInstance { get; set; }  // Navigation for CourseInstance (many-to-one)

        //Navigation for media (one-to-many)
        public virtual ICollection<Media> Sessionmedia { get; set; } = new List<Media>();

    }
}
