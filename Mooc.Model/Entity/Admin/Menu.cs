namespace Mooc.Model.Entity;

public class Menu : BaseEntity
{
    /// <summary>
    /// the title of the menu.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// the permission associated with the menu.
    /// </summary>
    public string? Permission { get; set; }

    /// <summary>
    /// the type of the menu.
    /// </summary>
    public MenuType MenuType { get; set; }

    /// <summary>
    /// Gets or sets the description of the menu.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// the order number for sorting the menu.
    /// Lower numbers are displayed first.
    /// </summary>
    public int OrderNum { get; set; } = 0;

    /// <summary>
    /// the route associated with the menu.
    /// </summary>
    public string? Route { get; set; }

    /// <summary>
    /// the component path for the menu.
    /// </summary>
    public string? ComponentPath { get; set; }

    /// <summary>
    /// the child menus of this menu.
    /// </summary>
    public virtual ICollection<Menu> Children { get; set; } = new List<Menu>();

    /// <summary>
    /// the parent menu ID.
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// the parent menu entity.
    /// </summary>
    public Menu? Parent { get; set; }

    /// <summary>
    /// the role-menu associations for this menu.
    /// </summary>
    public virtual ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();

}
