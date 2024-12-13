namespace Mooc.Application.Contracts.Admin;

public interface IMoocUserService : ICrudService<UserDto, UserDto, long, FilterPagedResultRequestDto, CreateUserDto, UpdateUserDto>
{
    Task<UserDto> GetByUserNameAsync(string userName);
}