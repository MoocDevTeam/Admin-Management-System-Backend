namespace Mooc.Model.Entity;


public class BaseEntity
{
    public long Id { get; set; }
}

/// <summary>
/// Audit for creating/updating
/// </summary>
public class BaseEntityWithAudit : BaseEntity
{
    public long? CreatedByUserId { get; set; }
    public long? UpdatedByUserId { get; set; }
     public DateTime? CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; }
}