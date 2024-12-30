namespace Mooc.Application.Contracts.Admin;

public class CreateOrUpdateRoleBaseDto : BaseEntityDto
{
    public string? RoleName { get; set; }

    public string? Description { get; set; }
}
