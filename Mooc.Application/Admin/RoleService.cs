
namespace Mooc.Application.Admin
{
  
    public class RoleService : CrudService<Role, RoleDto, RoleDto, long, FilterPagedResultRequestDto, CreateRoleDto, UpdateRoleDto>, IRoleService, ITransientDependency
    {
        public RoleService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<RoleDto> GetAsync(long id)
        {
            return await base.GetAsync(id);
        }

        public async Task<PagedResultDto<RoleDto>> GetListAsync(FilterPagedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }


        public async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            return await base.CreateAsync(input);
        }

        override public async Task<RoleDto> UpdateAsync(long id, UpdateRoleDto input)
        {
            return await base.UpdateAsync(id, input);
        }

        public async Task DeleteAsync(long id)
        {
            await base.DeleteAsync(id);
        }
    }
}
