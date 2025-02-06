namespace Mooc.Application.Contracts.Admin;

public class CreateOrUpdateRoleBaseDto : BaseEntityDto
{
    [Required(ErrorMessage = "RoleName cannnot be Null")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "UserName must be between 3 and 50 character")]
    public string? RoleName { get; set; }

    public string? Description { get; set; }
}
