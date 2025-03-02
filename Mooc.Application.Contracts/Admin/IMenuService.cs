using Mooc.Application.Contracts.Admin.Dto.Menu;

namespace Mooc.Application.Contracts.Admin
{
    public interface IMenuService : ICrudService<MenuDto, MenuDto, long, FilterPagedResultRequestDto, CreateMenuDto, UpdateMenuDto>
    {
        Task<List<MenuDto>> GetMenuTreeAsync();
        ListResultDto<OptionsDto> GetMenuType();

        Task<List<MenuIdDTO>> GetMenuIdsByRoleIdAsync(long roleId);
    }
}
