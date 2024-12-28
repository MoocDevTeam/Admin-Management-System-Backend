namespace Mooc.Application.Admin
{
    public class MenuService : CrudService<Menu, MenuDto, MenuDto, long, FilterPagedResultRequestDto, CreateMenuDto, UpdateMenuDto>, IMenuService, ITransientDependency
    {
        public MenuService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<MenuDto> GetAsync(long id)
        {
            return await base.GetAsync(id);
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
    }
}
