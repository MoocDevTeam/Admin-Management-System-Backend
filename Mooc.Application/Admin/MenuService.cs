using Microsoft.EntityFrameworkCore;

namespace Mooc.Application.Admin
{
    public class MenuService : CrudService<Menu, MenuDto, MenuDto, long, FilterPagedResultRequestDto, CreateMenuDto, UpdateMenuDto>, IMenuService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;
        public MenuService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
        }

        public async Task<MenuDto> GetAsync(long id)
        {
            var menu = await base.GetAsync(id);

            if(menu == null)
            {
                return null;
            }

            var allMenus = await GetAllMenusAsync();

            menu.Children = BuildMenuTree(allMenus, menu.Id);

            return menu;
        }

        public async Task<PagedResultDto<MenuDto>> GetListAsync(FilterPagedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }


        public async Task<MenuDto> CreateAsync(CreateMenuDto input)
        {
            return await base.CreateAsync(input);
        }

        public async Task<MenuDto> UpdateAsync(long id, UpdateMenuDto input)
        {
            return await base.UpdateAsync(id, input);
        }

        public async Task DeleteAsync(long id)
        {
            await base.DeleteAsync(id);
        }

        public async Task<List<MenuDto>> GetMenuTreeAsync()
        {
            var allMenus = await GetAllMenusAsync();

            return BuildMenuTree(allMenus, null);
        }

        private async Task<List<MenuDto>> GetAllMenusAsync()
        {
           
            var input = new FilterPagedResultRequestDto
            {
                PageSize = 100, 
                PageIndex = 1
            };

            var result = await GetListAsync(input);
            return result.Items.ToList();
        }

        private List<MenuDto> BuildMenuTree(List<MenuDto> menus, long? parentId)
        {
            return menus
                .Where(menu => menu.ParentId == parentId)
                .Select(menu => new MenuDto
                {
                    Id = menu.Id,
                    Title = menu.Title,
                    ParentId = menu.ParentId,
                    MenuType = menu.MenuType,
                    Description = menu.Description,
                    OrderNum = menu.OrderNum,
                    Route = menu.Route,
                    ComponentPath = menu.ComponentPath,
                    Permission = menu.Permission,
                    Children = BuildMenuTree(menus, menu.Id)
                })
                .OrderBy(menu => menu.OrderNum) 
                .ToList();
        }
    }
}
