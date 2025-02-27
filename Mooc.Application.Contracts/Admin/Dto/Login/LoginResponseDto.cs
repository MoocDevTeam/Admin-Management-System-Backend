using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Admin.Dto.Login;

/// <summary>
/// Result of login
/// </summary>
public class LoginResponseDto
{
    public string Token { get; set; }
    public List<string> Permissions { get; set; }
}
