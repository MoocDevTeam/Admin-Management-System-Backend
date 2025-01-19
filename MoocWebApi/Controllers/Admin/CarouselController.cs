
using Microsoft.AspNetCore.Mvc;

namespace MoocWebApi.Controllers.Admin
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.AdminService))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CarouselController : ControllerBase
    {
    }
}
