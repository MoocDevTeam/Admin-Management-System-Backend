using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Admin
{
    public interface IMenuService : ICrudService<CreateMenuDto, CreateMenuDto, long, FilterPagedResultRequestDto, CreateMenuDto, UpdateMenuDto>
    {
        Task<List<CreateMenuDto>> GetAllVisibleMenusAsync();
    }
}
