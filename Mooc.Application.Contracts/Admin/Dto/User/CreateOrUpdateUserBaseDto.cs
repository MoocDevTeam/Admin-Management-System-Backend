namespace Mooc.Application.Contracts.Admin.Dto;

public class CreateOrUpdateUserBaseDto : BaseEntityDto
{

    [Required(ErrorMessage = "UserName is null")]
    [StringLength(100, ErrorMessage = "UserName  must be less than 100 characters")]
    public string UserName { get; set; }

    // [Required(ErrorMessage = "Password is null")]
    // [StringLength(100, ErrorMessage = "Password  must be less than 100 characters")]
  
    public int Age { get; set; }

    public string? Email { get; set; }
    public Access Access { get; set; }
    public Gender Gender { get; set; }



    public DateTime? CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public List<long> RoleIds { get; set; } = new List<long>();
}
