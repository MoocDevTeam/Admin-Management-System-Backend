
using Microsoft.EntityFrameworkCore;

namespace Mooc.Application.Admin
{
  
    public class RoleService : CrudService<Role, RoleDto, RoleDto, long, FilterPagedResultRequestDto, CreateRoleDto, UpdateRoleDto>, IRoleService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;
        private readonly IMapper _mapper;

        public RoleService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

       override  public async Task<RoleDto> GetAsync(long id)
        {
            return await base.GetAsync(id);
        }

        public async Task<PagedResultDto<RoleDto>> GetAllRolesAsync(FilterPagedResultRequestDto input)
        {
            Console.Write($"input pageindex is: {input.PageIndex}");
            if (input.PageIndex < 1)
            {
                input.PageIndex = 1;
            }
            var query = _dbContext.Roles.AsQueryable();

            int totalItems = await query.CountAsync();

            var roles = await query.OrderBy(r => r.Id).Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize).ToListAsync();

            var roleDtos = _mapper.Map<List<RoleDto>>(roles);

            return new PagedResultDto<RoleDto>
            {
                Total = totalItems,
                Items = roleDtos
            };
            //return await base.GetListAsync(input);
        }

        public async Task<PagedResultDto<RoleDto>> GetListAsync(FilterPagedResultRequestDto input)
        {
           
            return await base.GetListAsync(input);
        }

        override public async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {
            return await base.CreateAsync(input);
        }

        override public async Task<RoleDto> UpdateAsync(long id, UpdateRoleDto input)
        {
            return await base.UpdateAsync(id, input);
        }

        override public async Task DeleteAsync(long id)
        {
            await base.DeleteAsync(id);
        }
    }
}
