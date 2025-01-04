using Microsoft.AspNetCore.Hosting;

namespace Mooc.Application.Course
{
    public class TeacherCourseInstanceService : CrudService<TeacherCourseInstance, TeacherCourseInstanceReadDto, TeacherCourseInstanceReadDto, long, FilterPagedResultRequestDto, TeacherCourseInstanceCreateOrUpdateDto, TeacherCourseInstanceCreateOrUpdateDto>, ITeacherCourseInstanceService, ITransientDependency
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
    
        public TeacherCourseInstanceService(MoocDBContext dBContext, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(dBContext, mapper)
        {
            this._webHostEnvironment = webHostEnvironment;
        }

        protected override IQueryable<TeacherCourseInstance> CreateFilteredQuery(FilterPagedResultRequestDto input)
        {
            if (!string.IsNullOrEmpty(input.Filter))
            {
                return GetQueryable().Where(t => t.Teacher.User.UserName.Contains(input.Filter));
            }

            return base.CreateFilteredQuery(input);
        }

        //update
        public override async Task<TeacherCourseInstanceReadDto> UpdateAsync(long id, TeacherCourseInstanceCreateOrUpdateDto input)
        {
            await ValidateTeacherCourseInstanceId(id);
            return await base.UpdateAsync(id, input);
        }

        //validate TeacherCourseInstanceId
        protected virtual async Task ValidateTeacherCourseInstanceId(long id)
        {
            var teacherCourseInstance = await this.McDBContext.TeacherCourseInstances.FirstOrDefaultAsync(x => x.Id == id);

            if (teacherCourseInstance == null)
            {
                throw new EntityNotFoundException($"No course instance found with id {id}");
            }
        }

        public async Task<TeacherCourseInstanceReadDto> GetTeacherCourseInstanceById(long id)
        {
            await ValidateTeacherCourseInstanceId(id);
            return await base.GetAsync(id);
        }
    }
}
 