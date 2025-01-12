namespace Mooc.Model.Entity;

public class User : BaseEntity
{    
    //To fix the conflicts and enable data migration. Team02 temporarily modifies this part. 
    public DateTime? CreatedAt { get; set; }
    public string UserName { get; set; }

    public string Password { get; set; }

    public string? Email { get; set; }

    public int? Age { get; set; }

    public Access Access { get; set; }

    public Gender Gender { get; set; }

    public string Avatar { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}