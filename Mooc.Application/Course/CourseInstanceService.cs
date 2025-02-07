namespace Mooc.Application.Course
{
    public class CourseInstanceService : CrudService<CourseInstance, CourseInstanceDto, CourseInstanceDto, long, FilterPagedResultRequestDto, CreateCourseInstanceDto, UpdateCourseInstanceDto>,
        ICourseInstanceService, ITransientDependency
    {
        public CourseInstanceService(MoocDBContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        /// <summary>
        /// Overrides the default MapToEntity method to invoke SetUpdatedAudit() while creating a new CourseInstance
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override CourseInstance MapToEntity(CreateCourseInstanceDto input)
        {
            var entity = this.Mapper.Map<CreateCourseInstanceDto, CourseInstance>(input);
            SetIdForLong(entity);
            SetCreatedAudit(entity);
            SetUpdatedAudit(entity);
            return entity;
        }

        /// <summary>
        /// Overrides the default CreateFilteredQuery method to include related entities while fetching CourseInstance
        /// </summary>
        /// <param name="input">Filter parameters for the query</param>
        /// <returns>IQueryable with related entities included</returns>
        protected override IQueryable<CourseInstance> CreateFilteredQuery(FilterPagedResultRequestDto input)
        {
            return this.McDBContext.CourseInstances
                .Include(x => x.MoocCourse)
                .Include(x => x.CreatedByUser)
                .Include(x => x.UpdatedByUser)
                .Include(x => x.TeacherCourseInstances)
                    .ThenInclude(tci => tci.Teacher)
                .Include(x => x.Sessions)
                .Include(x => x.Enrollment);
        }

        /// <summary>
        /// Fetches a CourseInstance by Id with related entities included
        /// </summary>
        /// <param name="id">Id of the CourseInstance to fetch</param>
        /// <returns>CourseInstanceDto</returns>
        /// <exception cref="EntityNotFoundException">Thrown when no CourseInstance is found with the specified Id</exception>
        public override async Task<CourseInstanceDto> GetAsync(long id)
        {
            var courseInstance = await this.McDBContext.CourseInstances
                .Include(x => x.MoocCourse)
                .Include(x => x.CreatedByUser)
                .Include(x => x.UpdatedByUser)
                .Include(x => x.TeacherCourseInstances)
                     .ThenInclude(tci => tci.Teacher)
                .Include(x => x.Sessions)
                .Include(x => x.Enrollment)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (courseInstance == null)
                throw new EntityNotFoundException("CourseInstance not Found", $"CourseInstance with Id {id} is not found");
            return MapToGetOutputDto(courseInstance);
        }

        //public async Task<PagedResultDto<CourseInstanceDto>> GetListAsync(FilterPagedResultRequestDto input)
        //{
        //    return await base.GetListAsync(input);
        //}


        //public async Task<CourseInstanceDto> CreateAsync(CreateCourseInstanceDto input)
        //{
        //    return await base.CreateAsync(input);
        //}

        //public async Task<CourseInstanceDto> UpdateAsync(long id, UpdateCourseInstanceDto input)
        //{
        //    return await base.UpdateAsync(id, input);
        //}

        //public async Task DeleteAsync(long id)
        //{
        //    await base.DeleteAsync(id);
        //}

        //public async Task<List<CourseInstanceDto>> GetByMoocCourseTtileAsync(string moocCourseTtile)
        //{
        //    var moocCourse = await this.McDBContext.MoocCourses.FirstOrDefaultAsync(x => x.Title.ToLower() == moocCourseTtile.ToLower());

        //    if (moocCourse == null)
        //    {
        //        return null;
        //    }
        //    //Fetch CourseInstances related to the found MoocCourse
        //    var courseInstances = await GetQueryable().Where(x => x.MoocCourseId == moocCourse.Id).ToListAsync();
        //    return MapToGetListOutputDtos(courseInstances);
        //}
    }
}
