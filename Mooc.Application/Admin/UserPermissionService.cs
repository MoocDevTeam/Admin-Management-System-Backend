
namespace Mooc.Application.Admin;

public class UserPermissionService : IUserPermissionService, ITransientDependency
{
    private readonly MoocDBContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMoocCache _moocCache;
    public UserPermissionService(MoocDBContext dbContext, IMapper mapper, IMoocCache moocCache)
    {
        this._dbContext = dbContext;
        this._mapper = mapper;
        this._moocCache = moocCache;
    }

    public async Task<List<string>> GetUserPermissListAsync(long userId)
    {
        var cacheKey = string.Format(CacheConsts.PermissCacheKey, userId);
        var permissList = await _moocCache.GetAsync<List<string>>(cacheKey);

        if (permissList == null)
        {
            var user = await this._dbContext.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return new List<string>();

            //没有角色
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
            await this._moocCache.SetAsync(cacheKey, permissList, 120);
        }

        return permissList;
    }
}
