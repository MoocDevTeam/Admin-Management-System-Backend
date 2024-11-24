using System.Security.Claims;

namespace Mooc.Core.Security;

public static class MoocClaimTypes
{
    /// <summary>
    /// Default: <see cref="ClaimTypes.Name"/>
    /// </summary>
    public static string UserName { get; set; } = ClaimTypes.Name;

    /// <summary>
    /// Default: <see cref="ClaimTypes.NameIdentifier"/>
    /// </summary>
    public static string UserId { get; set; } = ClaimTypes.NameIdentifier;
    public static string Email { get; set; } = "Email";


    /// <summary>
    /// Default: <see cref="ClaimTypes.Role"/>
    /// </summary>
    public static string Role { get; set; } = ClaimTypes.Role;
}
