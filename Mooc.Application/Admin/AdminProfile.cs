using Mooc.Application.Contracts.Admin.Dto.Menu;
using Mooc.Application.Contracts.Admin.Dto.User;

namespace Mooc.Application.Admin;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserWithRoleIdsDto>();
        CreateMap<CreateUserDto, User>().ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateUserDto, User>();
        CreateMap<Menu, MenuDto>();
        CreateMap<MenuIdDTO, Menu>().ReverseMap().ForMember(dest => dest.MenuId, opt => opt.MapFrom(src => src.Id));
        CreateMap<CreateMenuDto, Menu>();
        CreateMap<UpdateMenuDto, Menu>();
        CreateMap<Role, RoleDto>();
        CreateMap<CreateRoleDto, Role>();
        CreateMap<UpdateRoleDto, Role>();
        CreateMap<LoginRequestDto, User>();
    }
}
