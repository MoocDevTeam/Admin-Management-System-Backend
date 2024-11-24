using System.Security.Claims;

namespace Mooc.Core.Authorization;

public interface IPermissionChecker
{
    Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name);
}
