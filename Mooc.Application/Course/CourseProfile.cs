namespace Mooc.Application.Course;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<EnrollmentDto, Enrollment>();
        CreateMap<CreateEnrollmentDto, Enrollment>();
        CreateMap<UpdateEnrollmentDto, Enrollment>();

        CreateMap<CourseDto, MoocCourse>();
        CreateMap<MoocCourse, CourseDto>();
        CreateMap<CreateCourseDto, MoocCourse>();
        CreateMap<MoocCourse, CreateCourseDto>();
        CreateMap<UpdateCourseDto, MoocCourse>();

        //CreateMap<CreateUserDto, User>();
        //CreateMap<UpdateUserDto, User>();
    }
}

