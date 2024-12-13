using Microsoft.AspNetCore.Hosting;

namespace Mooc.Application.Admin;

public class MoocUserService : CrudService<MoocUser, UserDto, UserDto, long, FilterPagedResultRequestDto, CreateUserDto, UpdateUserDto>, 
    IMoocUserService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public MoocUserService(MoocDBContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(dbContext, mapper)
    {
        this._webHostEnvironment = webHostEnvironment;
    }
    public async Task<UserDto> GetByUserNameAsync(string userName)
    {
        var user = await this.McDBContext.MoocUsers.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user == null)
            return null;

        var userOutput = this.Mapper.Map<UserDto>(user);
        return userOutput;
    }
}