using Mooc.Core.Authorization;
using Mooc.Core.Security;
using System.Security.Claims;

namespace Mooc.Application.Authorization;

public class PermissionChecker : IPermissionChecker, ITransientDependency
{

    private readonly IUserPermissionService _userPermissionService;
    private readonly MoocDBContext _moocDBContext;
    public PermissionChecker(IUserPermissionService userPermissionService)
    {
        this._userPermissionService = userPermissionService;
    }
    public async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
    {
        var userId = claimsPrincipal?.FindFirst(MoocClaimTypes.UserId)?.Value;
        if (userId == null)
            return false;


        if (!long.TryParse(userId, out var userIdValue))
            return false;

        var permissList = await this._userPermissionService.GetUserPermissListAsync(userIdValue);
        return permissList.Contains(name);
    }
}
