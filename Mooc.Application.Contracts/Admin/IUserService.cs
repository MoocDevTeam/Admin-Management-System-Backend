using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Admin;

public interface IUserService : ICrudService<UserDto, UserDto, long, FilterPagedResultRequestDto, CreateUserDto, UpdateUserDto>
{
    Task<UserDto> GetByUserNameAsync(string userName);

}
