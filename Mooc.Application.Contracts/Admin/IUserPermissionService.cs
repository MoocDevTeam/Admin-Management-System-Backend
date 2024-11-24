

namespace Mooc.Application.Contracts.Admin;

public interface IUserPermissionService
{
    Task<List<string>> GetUserPermissListAsync(long userId);
    //Task<List<MenuDto>> GetUserMenuListAsync(long userId);
}
