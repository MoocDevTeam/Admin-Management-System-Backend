namespace Mooc.Model.Entity
{
    public class Session : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; } 
        public long CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }  
        public DateTime CreatedAt { get; set; }  
        public DateTime? UpdatedAt { get; set; }
        public virtual User CreatedByUser { get; set; }
        public virtual User UpdatedByUser { get; set; }
    }
}
