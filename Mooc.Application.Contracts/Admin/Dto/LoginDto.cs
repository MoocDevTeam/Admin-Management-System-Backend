using System.ComponentModel.DataAnnotations;

namespace Mooc.Application.Contracts.Admin;

/// <summary>
/// login input model
/// </summary>
public class LoginDto
{
    /// <summary>
    /// UserName
    /// </summary>
    [Required(ErrorMessage = "UserName is not null")]
    public string? UserName { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    [Required(ErrorMessage = "Password is not null")]
    public string? Password { get; set; }
}
