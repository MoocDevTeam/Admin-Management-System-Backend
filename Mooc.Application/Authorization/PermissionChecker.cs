using Mooc.Core.Authorization;
using Mooc.Core.Caching;
using Mooc.Core.Security;
using System.Security.Claims;

namespace Mooc.Application.Authorization;

public class PermissionChecker : IPermissionChecker, ITransientDependency
{

 
    private readonly MoocDBContext _moocDBContext;
    private readonly IMoocCache _moocCache;
    private readonly IRoleService _roleService;
    public PermissionChecker(MoocDBContext _moocDBContext, IMoocCache moocCache, IRoleService roleService)
    {
        this._moocDBContext = _moocDBContext;
        this._moocCache = moocCache;
        _roleService = roleService;
    }
    public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
    {
        var userId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return false;


        if (!long.TryParse(userId, out var userIdValue))
            return false;

        var permissList = await _roleService.GetPermissionsbyUserIdAsync(userIdValue);
        return permissList.Contains(name);
    }
}
