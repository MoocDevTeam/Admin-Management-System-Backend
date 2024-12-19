namespace Mooc.Application.Contracts.Admin;

public class UserDto :BaseEntityDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public Gender Gender { get; set; }

    public string? Avatar { get; set; }

    public DateTime? Created { get; set; }

}
