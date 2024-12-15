namespace Mooc.Application.Course
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
        }
    }
}
