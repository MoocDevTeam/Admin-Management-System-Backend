namespace Mooc.Model.Entity;

public class MoocUserRole : BaseEntity
{
    public MoocUser MoocUser { get; set; }

    public long MoocUserId { get; set; }

    public Role Role { get; set; }

    public long RoleId { get; set; }


}