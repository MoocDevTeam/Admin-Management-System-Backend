
using Microsoft.EntityFrameworkCore;

namespace Mooc.Application.Admin
{

    public class RoleService : CrudService<Role, RoleDto, RoleDto, long, FilterPagedResultRequestDto, CreateRoleDto, UpdateRoleDto>, IRoleService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMoocCache _moocCache;

        public RoleService(MoocDBContext dbContext, IMapper mapper, IMoocCache moocCache) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _moocCache = moocCache;
        }


        override public async Task<RoleDto> GetAsync(long id)
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

        protected override IQueryable<Role> CreateFilteredQuery(FilterPagedResultRequestDto input)
        {
            return base.CreateFilteredQuery(input);
        }


        public override async Task<RoleDto> CreateAsync(CreateRoleDto input)

        {
            return await base.CreateAsync(input);
        }

        public override async Task<RoleDto> UpdateAsync(long id, UpdateRoleDto input)
        {
            return await base.UpdateAsync(id, input);
        }

        public override async Task DeleteAsync(long id)

        {
            await base.DeleteAsync(id);
        }
        /// <summary>
        /// bulk detele role ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>true or false</returns>
        public async Task<bool> BulkDelete(List<long> ids)
        {
            var dbSet = this.GetDbSet();
            var roleList = await this.McDBContext.Roles.Where(x => ids.Contains(x.Id)).ToListAsync();
            if (roleList.Any())
                this.McDBContext.Roles.RemoveRange(roleList);
            var deleteList = await dbSet.Where(x => ids.Contains(x.Id)).ToListAsync();
            if (deleteList.Any())
                dbSet.RemoveRange(deleteList);
            await this.McDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetPermissionsbyUserIdAsync(long id)
        {
            var cacheKey = string.Format(CacheConsts.PermissCacheKey, id);
            var permissList = await _moocCache.GetAsync<List<string>>(cacheKey);

            if (permissList == null)
            {
                var user = await this._dbContext.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                    return new List<string>();

                //no roles
                if (user.UserRoles == null || user.UserRoles.Count == 0)
                    return new List<string>();
                var roleIds = user.UserRoles.Select(x => x.RoleId).ToList();
                permissList = new List<string>();
                var roleMenuList = await this._dbContext.RoleMenus.Include(r => r.Menu).Where(r => roleIds.Contains(r.RoleId)).ToListAsync();
                foreach (var roleMenu in roleMenuList)
                {
                    if (!string.IsNullOrEmpty(roleMenu.Menu.Permission))
                    {
                        if (!permissList.Contains(roleMenu.Menu.Permission))
                            permissList.Add(roleMenu.Menu.Permission);
                    }
                }
                await this._moocCache.SetAsync(cacheKey, permissList, 60);
            }

            return permissList;
        }

        /// <summary>
        /// Save permissions for a sigle role
        /// </summary>
        /// <param name="id">Role id</param>
        /// <param name="menuIdList">permission list</param>
        /// <returns></returns>
        public async Task<bool> RolePermissionAsync(long id, List<long> menuIdList)
        {
            var oldRoleMenuList = await this.McDBContext.RoleMenus.Where(x => x.RoleId == id).ToListAsync();
            this.McDBContext.RoleMenus.RemoveRange(oldRoleMenuList);
            foreach (var iMenuId in menuIdList)
            {
                var roleMenu = new RoleMenu() { MenuId = iMenuId, RoleId = id };
                SetIdForLong(roleMenu);
                this.McDBContext.RoleMenus.Add(roleMenu);
            }
            var isSuccesss = await this.McDBContext.SaveChangesAsync() > 0;

            if (isSuccesss)
            {
                var userIdList = this.McDBContext.UserRoles.Where(x => x.RoleId == id).Select(x => x.UserId).Distinct();
                foreach (var userId in userIdList)
                {
                    var cacheKey = string.Format(CacheConsts.PermissCacheKey, userId);
                    await _moocCache.RemoveAsync(cacheKey);
                }

            }
            return isSuccesss;
        }
    }
}
