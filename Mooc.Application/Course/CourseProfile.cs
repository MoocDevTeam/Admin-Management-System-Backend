﻿using Mooc.Application.Contracts.Course.Dto;

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
        CreateMap<Session, ReadSessionDto>();
        
        // CourseInstance Mapping
        CreateMap<CourseInstance, CourseInstanceDto>();
        CreateMap<CreateCourseInstanceDto, CourseInstance>();
        CreateMap<UpdateCourseInstanceDto, CourseInstance>();
    }
}

