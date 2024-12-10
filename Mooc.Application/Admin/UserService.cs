using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mooc.Core.Utils;

namespace Mooc.Application.Admin;

public class UserService : CrudService<User, UserDto, UserDto, long, FilterPagedResultRequestDto, CreateUserDto, UpdateUserDto>,
        IUserService, ITransientDependency
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    public UserService(MoocDBContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(dbContext, mapper)
    {
        this._webHostEnvironment = webHostEnvironment;
    }

    protected override IQueryable<User> CreateFilteredQuery(FilterPagedResultRequestDto input)
    {
        if (!string.IsNullOrEmpty(input.Filter))
        {
            return GetQueryable().Where(x => x.UserName.Contains(input.Filter));
        }
        return base.CreateFilteredQuery(input);
    }

    public override async Task<UserDto> CreateAsync(CreateUserDto input)
    {
        await ValidateNameAsync(input.UserName, 0);

        input.Password = BCryptUtil.HashPassword(input.Password);
        var userDto = await base.CreateAsync(input);
        return userDto;

    }

    public override async Task<UserDto> UpdateAsync(long id, UpdateUserDto input)
    {
        await ValidateNameAsync(input.UserName, id);
        return await base.UpdateAsync(id, input);
    }

    protected virtual async Task ValidateNameAsync(string userName, long? expectedId = null)
    {
        var category = await this.GetQueryable().FirstOrDefaultAsync(p => p.UserName == userName);
        if (category != null && category.Id != expectedId)
        {
            throw new EntityAlreadyExistsException($"User {userName} already exists", $"{userName} already exists");
        }
    }

    public async Task<UserDto> GetByUserNameAsync(string userName)
    {
        var user = await this.McDBContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        if (user == null)
            return null;

        var userOutput = this.Mapper.Map<UserDto>(user);
        return userOutput;
    }

}
