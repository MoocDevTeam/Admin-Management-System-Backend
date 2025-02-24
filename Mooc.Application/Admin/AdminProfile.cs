﻿namespace Mooc.Application.Admin;

public class AdminProfile : Profile
{
    public AdminProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<CreateUserDto, User>().ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateUserDto, User>();
        CreateMap<Menu, MenuDto>();
        CreateMap<CreateMenuDto, Menu>();
        CreateMap<UpdateMenuDto, Menu>();
        CreateMap<Role, RoleDto>();
        CreateMap<CreateRoleDto, Role>();
        CreateMap<UpdateRoleDto, Role>();
        CreateMap<LoginRequestDto, User>();
    }
}
