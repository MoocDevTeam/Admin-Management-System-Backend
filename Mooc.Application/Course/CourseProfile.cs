using Mooc.Application.Contracts.Course.Dto.Category;
using Mooc.Model.Entity.Course;

namespace Mooc.Application.Course;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        CreateMap<EnrollmentDto, Enrollment>();
        CreateMap<Enrollment, EnrollmentDto>();
        CreateMap<CreateEnrollmentDto, Enrollment>();
        CreateMap<Enrollment, CreateEnrollmentDto>();
        CreateMap<UpdateEnrollmentDto, Enrollment>();
        CreateMap<Enrollment, UpdateEnrollmentDto>();

        CreateMap<CourseDto, MoocCourse>();
        CreateMap<MoocCourse, CourseDto>();
        CreateMap<CreateCourseDto, MoocCourse>();
        CreateMap<MoocCourse, CreateCourseDto>();
        CreateMap<UpdateCourseDto, MoocCourse>();

        CreateMap<CategoryDto, Category>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<Category, CreateCategoryDto>();
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<Category, UpdateCategoryDto>();

        //CreateMap<CreateUserDto, User>();
        //CreateMap<UpdateUserDto, User>();
    }
}

