namespace Mooc.Application.Contracts.Admin;

public class LoginRequestDto : BaseEntityDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
