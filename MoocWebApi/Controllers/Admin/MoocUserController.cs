using Microsoft.AspNetCore.Authorization;

namespace MoocWebApi.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.AdminService))]

    [Route("api/[controller]/[action]")]
    [ApiController]

}