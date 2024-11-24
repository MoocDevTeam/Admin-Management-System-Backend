namespace Mooc.Model.Entity;

public class Role: BaseEntity
{
    public string RoleName { get; set; }

    public string? Mrak { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }

    public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();

}
