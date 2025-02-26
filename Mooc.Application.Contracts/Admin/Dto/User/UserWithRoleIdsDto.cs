using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Admin.Dto.User;

public class UserWithRoleIdsDto: UserDto
{
    public List<long> RoleIds { get; set; } = new List<long>();
}
