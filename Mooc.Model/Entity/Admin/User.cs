

namespace Mooc.Model.Entity;

public class User : BaseEntity
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public int Age { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Avatar { get; set; } 

    public Gender Gender { get; set; }

    public DateTime? Created { get; set; }
}
