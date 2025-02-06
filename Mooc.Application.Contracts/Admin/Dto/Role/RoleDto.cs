using Mooc.Model.Entity;
using Mooc.Shared.Enum;

namespace Mooc.Application.Contracts.Admin
{
    public class RoleDto: BaseEntityDto
    {
        [Required(ErrorMessage = "RoleName cannnot be Null")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "UserName must be between 3 and 50 character")]
        public string? RoleName { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
