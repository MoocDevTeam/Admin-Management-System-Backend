namespace Mooc.Application.Course;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<EnrollmentDto, Enrollment>();
        CreateMap<CreateEnrollmentDto, Enrollment>();
        CreateMap<UpdateEnrollmentDto, Enrollment>();
        //CreateMap<CreateUserDto, User>();
        //CreateMap<UpdateUserDto, User>();
    }
}

