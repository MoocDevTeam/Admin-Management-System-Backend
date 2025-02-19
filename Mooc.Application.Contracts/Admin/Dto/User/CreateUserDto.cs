namespace Mooc.Application.Contracts.Admin;

public class CreateUserDto : CreateOrUpdateUserBaseDto
{
    public string? Avatar { get; set; }

    [Required(ErrorMessage = "Password is null")]
    [StringLength(100, ErrorMessage = "Password  must be less than 100 characters")]
    public virtual string Password { get; set; }
}
