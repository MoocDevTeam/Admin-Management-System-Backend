using Mooc.Application.Contracts.Course.Dto.Category;
using Mooc.Application.Contracts.Course.Dto;

namespace Mooc.Application.Course;

public class CourseProfile : Profile
{
    public CourseProfile()
    {

        CreateMap<Enrollment, EnrollmentDto>();
        CreateMap<CreateEnrollmentDto, Enrollment>();
        CreateMap<UpdateEnrollmentDto, Enrollment>();

        CreateMap<CourseDto, MoocCourse>();
        CreateMap<MoocCourse, CourseDto>()
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
         .ForMember(dest => dest.CourseInstances, opt => opt.MapFrom(src => src.CourseInstances));
        CreateMap<CreateCourseDto, MoocCourse>();
        CreateMap<MoocCourse, CreateCourseDto>();
        CreateMap<UpdateCourseDto, MoocCourse>();

        CreateMap<Category, CategoryDto>()
                 .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();

        //teacher mapping
        //Frontend---> Database
        CreateMap<CreateOrUpdateTeacherDto, Teacher>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedByUserId, opt => opt.Ignore());
        //Database ---> Frontend
        CreateMap<Teacher, TeacherReadDto>()
            .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.UserName : null))
            .ForMember(dest => dest.UpdatedByUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.UserName : null));

        //TeacherCourseInstance mapping
        //Frontend---> Database
        CreateMap<TeacherCourseInstanceCreateOrUpdateDto, TeacherCourseInstance>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedByUserId, opt => opt.Ignore());

        //Database ---> Frontend
        CreateMap<TeacherCourseInstance, TeacherCourseInstanceReadDto>()
            .ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.UserName : null))
            .ForMember(dest => dest.UpdatedByUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? src.UpdatedByUser.UserName : null));

        CreateMap<TeacherCourseInstance, TeacherCourseInstancePermissionDto>();

        // Session Mapping
        // Frontend -> Backend -> Database 
        CreateMap<CreateOrUpdateSessionDto, Session>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        //  Database  -> Backend -> Frontend
        CreateMap<Session, ReadSessionDto>()
             .ForMember(dest => dest.Media, opt => opt.MapFrom(src => src.Sessionmedia))
            .ForMember(dest => dest.MediaCount, opt => opt.MapFrom(src => src.Sessionmedia.Count))
            .ForMember(dest => dest.HasMedia, opt => opt.MapFrom(src => src.Sessionmedia.Any()));

        // CourseInstance Mapping
        CreateMap<CourseInstance, CourseInstanceDto>()
         .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => src.TeacherCourseInstances.Select(tci => tci.Teacher)))
        .ForMember(dest => dest.Sessions, opt => opt.MapFrom(src => src.Sessions))
         .ForMember(dest => dest.Enrollment, opt => opt.MapFrom(src => src.Enrollment));
        CreateMap<CreateCourseInstanceDto, CourseInstance>();
        CreateMap<UpdateCourseInstanceDto, CourseInstance>();

        CreateMap<Media, MediaDto>();
    }
}

