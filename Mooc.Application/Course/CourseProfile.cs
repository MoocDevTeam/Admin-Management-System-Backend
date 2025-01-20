using Mooc.Application.Contracts.Course.Dto;

namespace Mooc.Application.Course;

public class CourseProfile : Profile
{
    public CourseProfile()
    {

        CreateMap<Enrollment, EnrollmentDto>();
        CreateMap<CreateEnrollmentDto, Enrollment>();
        CreateMap<UpdateEnrollmentDto, Enrollment>();

        CreateMap<MoocCourse, CourseDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));
            //.ForMember(dest => dest.CourseInstances, opt => opt.MapFrom(src => src.CourseInstances));
        CreateMap<CreateCourseDto, MoocCourse>();
        CreateMap<MoocCourse, CreateCourseDto>();
        CreateMap<UpdateCourseDto, MoocCourse>();

        CreateMap<Category, CategoryDto>()
                 .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses))
                 .ForMember(dest => dest.ChildrenCategories, opt => opt.MapFrom(src => src.ChildrenCategories));
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
        CreateMap<CreateSessionDto, Session>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        CreateMap<UpdateSessionDto, Session>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        //  Database  -> Backend -> Frontend
        CreateMap<Session, ReadSessionDto>();
        //.ForMember(dest => dest.MediaFiles, opt => opt.MapFrom(src => src.Sessionmedia))
        //.ForMember(dest => dest.MediaCount, opt => opt.MapFrom(src => src.Sessionmedia.Count));

        //Media Mapping
        // Frontend -> Backend -> Database 
        CreateMap<CreateMediaDto, Media>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        CreateMap<UpdateMediaDto, Media>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        //  Database  -> Backend -> Frontend
        CreateMap<Media, ReadMediaDto>();

        #region //CourseInstance Mapping

        CreateMap<CourseInstance, CourseInstanceDto>()
            .ForMember(dest => dest.MoocCourseTitle, opt => opt.MapFrom(src => src.MoocCourse.Title))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.Permission.ToString()))
            .ForMember(dest => dest.Teachers, opt => opt.MapFrom(src => src.TeacherCourseInstances.Select(tci => tci.Teacher)))
            .ForMember(dest => dest.Sessions, opt => opt.MapFrom(src => src.Sessions))
            .ForMember(dest => dest.TotalSessions, opt => opt.MapFrom(src => src.Sessions.Count()))
            .ForMember(dest => dest.Enrollment, opt => opt.MapFrom(src => src.Enrollment))
            .ForMember(dest => dest.CreatedUserName, opt => opt.MapFrom(src => src.CreatedByUser.UserName))
            .ForMember(dest => dest.UpdatedUserName, opt => opt.MapFrom(src => src.UpdatedByUser.UserName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        CreateMap<CreateCourseInstanceDto, CourseInstance>();
        CreateMap<UpdateCourseInstanceDto, CourseInstance>();
        #endregion

        CreateMap<Media, MediaDto>();
    }
}

