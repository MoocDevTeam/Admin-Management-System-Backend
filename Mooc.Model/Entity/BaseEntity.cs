namespace Mooc.Model.Entity;


public class BaseEntity 
{
    public long Id { get; set; }
/*    public long? CreatedByUserId { get; set; }*/
    public DateTime? CreatedAt { get; set; }
/*    public long? UpdatedByUserId { get; set; }*/
    public DateTime? UpdatedAt { get; set; }
/*    public User CreatedByUser { get; set; }
    public virtual ICollection<User> UpdatedByUsers { get; set; }*/
}
