using Mooc.Application.Contracts.Course.Dto.Category;
using Mooc.Model.Entity.Course;

namespace Mooc.Application.Course;

public class CourseProfile : Profile
{
    public CourseProfile()
    {

        CreateMap<Enrollment, EnrollmentDto>();
        CreateMap<CreateEnrollmentDto, Enrollment>();
        CreateMap<UpdateEnrollmentDto, Enrollment>();


        CreateMap<CourseDto, MoocCourse>();
        CreateMap<MoocCourse, CourseDto>();
        CreateMap<CreateCourseDto, MoocCourse>();
        CreateMap<MoocCourse, CreateCourseDto>();
        CreateMap<UpdateCourseDto, MoocCourse>();


        CreateMap<Category, CategoryDto>()
                 .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();


        //CreateMap<CreateUserDto, User>();
        //CreateMap<UpdateUserDto, User>();
    }
}

