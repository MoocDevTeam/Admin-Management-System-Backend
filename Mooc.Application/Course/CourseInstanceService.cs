

namespace Mooc.Application.Course
{
    public class CourseInstanceService : CrudService<CourseInstance, CourseInstanceDto, CourseInstanceDto, long, FilterPagedResultRequestDto, CreateCourseInstanceDto, UpdateCourseInstanceDto>,
        ICourseInstanceService, ITransientDependency
    {
        public CourseInstanceService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public Task<CourseInstanceDto> GetByMoocCourseTtile(string moocCourseTtile)
        {
            //return 
        }
        

        //protected override IQueryable<User> CreateFilteredQuery(FilterPagedResultRequestDto input)
        //{
        //    if (!string.IsNullOrEmpty(input.Filter))
        //    {
        //        return GetQueryable().Where(x => x.UserName.Contains(input.Filter));
        //    }
        //    return base.CreateFilteredQuery(input);
        //}
    }
}
