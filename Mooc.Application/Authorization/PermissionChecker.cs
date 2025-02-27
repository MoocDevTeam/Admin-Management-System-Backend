using Mooc.Core.Authorization;
using Mooc.Core.Caching;
using Mooc.Core.Security;
using System.Security.Claims;

namespace Mooc.Application.Authorization;

public class PermissionChecker : IPermissionChecker, ITransientDependency
{

 
    private readonly MoocDBContext _moocDBContext;
    private readonly IMoocCache _moocCache;
    public PermissionChecker(MoocDBContext _moocDBContext, IMoocCache moocCache)
    {
        this._moocDBContext = _moocDBContext;
        this._moocCache = moocCache;
    }
    public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
    {
        var userId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return false;


        if (!long.TryParse(userId, out var userIdValue))
            return false;

        var permissList = await GetUserPermissListAsync(userIdValue);
        return permissList.Contains(name);
    }

    /// <summary>
    /// Get permissions based on the current users
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<string>> GetUserPermissListAsync(long userId)
    {
        var cacheKey = string.Format(CacheConsts.PermissCacheKey, userId);
        var permissList = await _moocCache.GetAsync<List<string>>(cacheKey);

        if (permissList == null)
        {
            var user = await this._moocDBContext.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return new List<string>();

            //no roles
            if (user.UserRoles == null || user.UserRoles.Count == 0)
                return new List<string>();


            var roleIds = user.UserRoles.Select(x => x.RoleId).ToList();
            permissList = new List<string>();
            var roleMenuList = await this._moocDBContext.RoleMenus.Include(r => r.Menu).Where(r => roleIds.Contains(r.RoleId)).ToListAsync();
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

}
