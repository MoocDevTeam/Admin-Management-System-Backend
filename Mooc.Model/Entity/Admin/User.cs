namespace Mooc.Model.Entity;

public class User : BaseEntity
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string? Email { get; set; }

    public int? Age { get; set; }

    public Access Access { get; set; }

    public Gender Gender { get; set; }

    public string Avatar { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
}