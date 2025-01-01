namespace Mooc.Model.Entity;

public class Role : BaseEntity
{
    public string RoleName { get; set; } 

    public string Description { get; set; }

    public virtual ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();


}