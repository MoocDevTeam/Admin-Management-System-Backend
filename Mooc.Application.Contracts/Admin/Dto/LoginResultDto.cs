namespace Mooc.Application.Contracts.Admin;

public class LoginResultDto
{
    public long UserId { get; set; }
    public string UserName { get; set; } 
    public string Email {  get; set; }  
    public string AssceToken { get; set; }

    public int ExpiresTime{ get; set; }

   // public List<string> PermissionList { get; set; } = new List<string>();
}
