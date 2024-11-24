namespace Mooc.Model.Entity;

public class RoleMenu : BaseEntity
{
    public long RoleId { get; set; }
    public Role Role { get; set; }
    public long MenuId { get; set; }
    public Menu Menu { get; set; }
}
