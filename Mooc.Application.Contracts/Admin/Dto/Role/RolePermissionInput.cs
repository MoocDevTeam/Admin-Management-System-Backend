using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Admin.Dto.Role
{
    public class RolePermissionInput
    {
        public long Id { get; set; }

        public List<long> MenuIds { get; set; }
    }
}
