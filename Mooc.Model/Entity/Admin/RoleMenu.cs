namespace Mooc.Model.Entity;

public class RoleMenu : BaseEntity
{

    public Menu Menu { get; set; }

    public long MenuId { get; set; }

    public Role Role { get; set; }

    public long RoleId { get; set; }


}