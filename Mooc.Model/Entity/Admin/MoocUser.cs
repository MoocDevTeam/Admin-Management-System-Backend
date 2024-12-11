namespace Mooc.Model.Entity;

public class MoocUser : BaseEntity
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string? Email { get; set; }

    public int? Age { get; set; }

    public Access Access { get; set; }

    public Gender Gender { get; set; }

    public string Avatar { get; set; }

    public DateTime? createdDate { get; set; }

    public bool isActive { get; set; }

    public ICollection<MoocUserRole> MoocUserRole { get; set; } = new List<MoocUserRole>();
}