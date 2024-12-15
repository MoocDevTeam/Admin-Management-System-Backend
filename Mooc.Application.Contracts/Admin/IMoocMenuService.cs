using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Admin
{
    public interface IMoocMenuService : ICrudService<MoocCreateMenuDto, MoocCreateMenuDto, long, FilterPagedResultRequestDto, MoocCreateMenuDto, MoocUpdateMenuDto>
    {
        Task<List<MoocCreateMenuDto>> GetAllVisibleMenusAsync();
    }
}
