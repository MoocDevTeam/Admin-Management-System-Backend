using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Admin
{
    public interface IRoleService : ICrudService<CreateRoleDto, long, FilterPagedResultRequestDto,  UpdateRoleDto>
    {
        Task<List<CreateRoleDto>> GetAllRolesAsync();
    }
}
