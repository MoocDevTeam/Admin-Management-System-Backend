using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Mooc.Application.Contracts.Admin.Dto;
using Mooc.Core.ExceptionHandling;
using Mooc.Core.Utils;
using Mooc.Model.Option;
using static System.Net.Mime.MediaTypeNames;

namespace Mooc.Application.Admin;

public class UserService : CrudService<User, UserDto, UserDto, long, FilterPagedResultRequestDto, CreateUserDto, UpdateUserDto>,
        IUserService, ITransientDependency
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly UploadFolderOption _uploadFolderOption;
    public UserService(MoocDBContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment, IOptions<UploadFolderOption> uploadFolderOption) : base(dbContext, mapper)
    {
        this._webHostEnvironment = webHostEnvironment;
        this._uploadFolderOption = uploadFolderOption.Value;
    }


    public override async Task<UserDto> CreateAsync(CreateUserDto input)
    {
        await ValidateNameAsync(input.UserName, 0);
        if (!string.IsNullOrEmpty(input.Avatar))
        {
            try
            {
                // 查找前缀的结束位置，即 "base64," 之后的位置
                int base64StartIndex = input.Avatar.IndexOf("base64,") + "base64,".Length;

                // 提取纯粹的 Base64 编码数据
                string base64WithoutPrefix = input.Avatar.Substring(base64StartIndex);

                byte[] imageBytes = Convert.FromBase64String(base64WithoutPrefix);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {

                    string format = GetImageFormat(imageBytes);
                    var fileName = $"{input.UserName}_{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.{format}";
                    var savePath = Path.Combine(this._webHostEnvironment.ContentRootPath, _uploadFolderOption.RootFolder, _uploadFolderOption.AvatarFolder);
                    var savePathFile = Path.Combine(savePath, fileName);

                    if (!Directory.Exists(savePath))
                        Directory.CreateDirectory(savePath);
                    await File.WriteAllBytesAsync(savePathFile, imageBytes);

                    var avatar = $"/{_uploadFolderOption.AvatarFolder}/{fileName}";
                    input.Avatar = avatar;
                }
            }
            catch (Exception ex)
            {
                ExThrow.Throw(ex.Message, "failed to save avatar");
            }
        }

        //role

        using (var transaction = await this.McDBContext.Database.BeginTransactionAsync())
        {
            try
            {
                input.Password=BCryptUtil.HashPassword(input.Password);
                var userDto = await base.CreateAsync(input);
                if (userDto.Id > 0)
                {
                    foreach (var iRoleId in input.RoleIds)
                    {
                        var userRole = new UserRole() { RoleId = iRoleId, UserId = userDto.Id };
                        SetIdForLong(userRole);
                        this.McDBContext.UserRoles.Add(userRole);
                    }
                    await this.McDBContext.SaveChangesAsync();
                }
                await transaction.CommitAsync();
                return userDto;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ExThrow.Throw(ex.Message, "save user falit");
            }
        }

        return new UserDto();
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
