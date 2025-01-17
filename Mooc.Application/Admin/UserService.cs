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

    /// <summary>
    /// Creates a new user and assigns roles (if provided).
    /// Includes validation for duplicate usernames and invalid RoleIds.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public override async Task<UserDto> CreateAsync(CreateUserDto input)
    {
        await ValidateNameAsync(input.UserName, 0);

        input.Password = BCryptUtil.HashPassword(input.Password);

        var user = this.Mapper.Map<User>(input);

        await ValidateRoleIdsAsync(input.RoleIds);

        if (input.RoleIds != null && input.RoleIds.Count > 0)
        {
            user.UserRoles = input.RoleIds.Select(roleId => new UserRole { RoleId = roleId, User = user }).ToList();
        }

        await this.McDBContext.Users.AddAsync(user);
        await this.McDBContext.SaveChangesAsync();

        var userDto = this.Mapper.Map<UserDto>(user);
        return userDto;

    }

    /// <summary>
    /// Validates that all provided Role IDs exist in the database.
    /// Throws an exception if any invalid IDs are found.
    /// </summary>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task ValidateRoleIdsAsync(IEnumerable<long> roleIds)
    {
        if (roleIds == null || roleIds.Count() == 0) return;

        var validRoleIds = await this.McDBContext.Roles.Select(r => r.Id).ToListAsync();

        //identify any invalid IDS
        var invalidRoleIds = roleIds.Except(validRoleIds).ToList();
        if (invalidRoleIds.Any())
        {
            throw new InvalidOperationException($"the following role ids are invalid:{string.Join(",", invalidRoleIds)}");
        }
    }

    /// <summary>
    ///  Updates an existing user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public override async Task<UserDto> UpdateAsync(long id, UpdateUserDto input)
    {
        await ValidateNameAsync(input.UserName, id);

        //Retrieve the existing user (with roles) from the database
        var user = await this.McDBContext.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) throw new EntityNotFoundException(nameof(User), id.ToString());

        //Map updated properties from the DTO to the User entity
        this.Mapper.Map(input, user);

        await ValidateRoleIdsAsync(input.RoleIds);

        if (input.RoleIds == null || !input.RoleIds.Any())
        {
            // If no roles are provided, clear all
            user.UserRoles.Clear();
        }
        else
        {
            // Determine which roles to add/remove
            var currentRoleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();
            var rolesToAdd = input.RoleIds.Except(currentRoleIds).ToList();
            var rolesToRemove = currentRoleIds.Except(input.RoleIds).ToList();

            foreach (var roleId in rolesToAdd)
            {
                user.UserRoles.Add(new UserRole { RoleId = roleId, UserId = id });
            }

            user.UserRoles = user.UserRoles.Where(ur => !rolesToRemove.Contains(ur.RoleId)).ToList();
        }

            await this.McDBContext.SaveChangesAsync();

            var userDto = this.Mapper.Map<UserDto>(user);
            return userDto;



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
        {
            throw new EntityNotFoundException($"User {userName} not found", $"{userName} not found");
        }

        var userOutput = this.Mapper.Map<UserDto>(user);
        return userOutput;
    }



    /// <summary>
    /// delete user with cascade delete of user roles
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    public async Task<bool> DeleteAsync(long id)
    {
        var user = await this.McDBContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        if(user == null)
        {
            throw new EntityNotFoundException(nameof(User), id.ToString());
        }
        this.McDBContext.Users.Remove(user);
        await this.McDBContext.SaveChangesAsync();
        return true;
    }
}
