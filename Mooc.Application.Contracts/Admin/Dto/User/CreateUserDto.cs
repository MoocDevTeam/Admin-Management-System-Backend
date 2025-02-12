namespace Mooc.Application.Contracts.Admin;

public class CreateUserDto : CreateOrUpdateUserBaseDto
{
    public string? Avatar { get; set; }
    public virtual string Password { get; set; }
}
