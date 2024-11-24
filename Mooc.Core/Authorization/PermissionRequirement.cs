using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Mooc.Core.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string PermissionName { get; }

    public PermissionRequirement([NotNull] string permissionName)
    {

        PermissionName = permissionName;
    }
}
