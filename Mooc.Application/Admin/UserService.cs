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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="imageBytes"></param>
    /// <returns></returns>
    public string GetImageFormat(byte[] imageBytes)
    {
        // Attempt to match common image formats
        using (MemoryStream ms = new MemoryStream(imageBytes))
        {
            BinaryReader reader = new BinaryReader(ms);
            byte[] buffer = reader.ReadBytes(8);

            if (buffer.Length < 2)
                return "";

            // JPEG (JFIF)
            if (buffer[0] == 0xFF && buffer[1] == 0xD8)
                return "Jpeg";

            // PNG
            if (buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47)
                return "Png";

            // GIF
            if (buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x38)
                return "Gif";

            // BMP
            if (buffer[0] == 0x42 && buffer[1] == 0x4D)
                return "Bmp";

            // TIFF
            if (buffer[0] == 0x49 && buffer[1] == 0x49 && (buffer[2] == 0x2A || buffer[2] == 0x4A))
                return "Tiff";

            return "jpeg";
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
