namespace Mooc.Application.Course
{
    public class CourseInstanceService : CrudService<CourseInstance, CourseInstanceDto, CourseInstanceDto, long, FilterPagedResultRequestDto, CreateCourseInstanceDto, UpdateCourseInstanceDto>,
        ICourseInstanceService, ITransientDependency
    {
        public CourseInstanceService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<CourseInstanceDto> GetAsync(long id)
        {
            return await base.GetAsync(id);
        }

        public async Task<PagedResultDto<CourseInstanceDto>> GetListAsync(FilterPagedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }


        public async Task<CourseInstanceDto> CreateAsync(CreateCourseInstanceDto input)
        {
            return await base.CreateAsync(input);
        }

        public async Task<CourseInstanceDto> UpdateAsync(long id, UpdateCourseInstanceDto input)
        {
            return await base.UpdateAsync(id, input);
        }

        public async Task DeleteAsync(long id)
        {
            await base.DeleteAsync(id);
        }

        public async Task<List<CourseInstanceDto>> GetByMoocCourseTtileAsync(string moocCourseTtile)
        {
            var moocCourse = await this.McDBContext.MoocCourses.FirstOrDefaultAsync(x => x.Title == moocCourseTtile);

            if (moocCourse == null)
            {
                return null;
            }
            //Fetch CourseInstances related to the found MoocCourse
            var courseInstances = await GetQueryable().Where(x => x.MoocCourseId == moocCourse.Id).ToListAsync();
            return MapToGetListOutputDtos(courseInstances);
        }
    }
}
