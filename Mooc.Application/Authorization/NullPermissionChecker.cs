using Mooc.Core.Authorization;
using System.Security.Claims;

namespace Mooc.Application.Authorization;

public class NullPermissionChecker : IPermissionChecker, ITransientDependency
{

    public Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
    {
        return Task.FromResult(true);
    }
}
