using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Admin
{
    public interface IRoleService : ICrudService<RoleDto, RoleDto, long, FilterPagedResultRequestDto, CreateRoleDto, UpdateRoleDto>
    {
     //   Task<List<CreateRoleDto>> GetAllRolesAsync();
    }
}