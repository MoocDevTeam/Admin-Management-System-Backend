using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooc.Application.Contracts.Course;

using Mooc.Core.Utils;
using Microsoft.AspNetCore.Hosting;

namespace Mooc.Application.Course
{

    public class MoocCourseService : CrudService<MoocCourse, CourseDto, CourseDto, long, FilterPagedResultRequestDto, CreateCourseDto, UpdateCourseDto>,
    IMoocCourseService, ITransientDependency
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public MoocCourseService(MoocDBContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(dbContext, mapper)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._mapper = mapper;
        }

        protected override IQueryable<MoocCourse> CreateFilteredQuery(FilterPagedResultRequestDto input)
        {
            if (!string.IsNullOrEmpty(input.Filter))
            {
                return GetQueryable().Where(x => x.Title.Contains(input.Filter) || x.CourseCode.Contains(input.Filter));
            }
            return base.CreateFilteredQuery(input);
        }

        public override async Task<CourseDto> CreateAsync(CreateCourseDto input)
        {

            var courseDto = await base.CreateAsync(input);
            return courseDto;
        }


        public override async Task<CourseDto> UpdateAsync(long id, UpdateCourseDto input)
        {
            await ValidateCourseNameAsync(input.CourseCode, id);
            return await base.UpdateAsync(id, input);
        }

        protected virtual async Task ValidateCourseNameAsync(string courseName, long? expectedId = null)
        {
            var course = await this.GetQueryable().FirstOrDefaultAsync(c => c.Title == courseName);
            if (course != null)
            {
                throw new EntityAlreadyExistsException($"Course with code {courseName} already exists", $"{courseName} already exists");
            }
        }

        public async Task<CourseDto> GetByCourseNameAsync(string courseName)
        {
            var course = await this.McDBContext.MoocCourses.FirstOrDefaultAsync(x => x.Title == courseName);
            if (course == null)
                return null;

            var courseOutput = this.Mapper.Map<CourseDto>(course);
            return courseOutput;
        }




        public async Task<List<CourseDto>> GetAllAsync()
        {
            var courses = await this.McDBContext.MoocCourses.ToListAsync();

            if (courses.Count == 0)
                return new List<CourseDto>();
            var courseOutput = this._mapper.Map<List<CourseDto>>(courses);
            return courseOutput;

        }

        public async Task<bool> CourseExist(string title)
        {
            // return _context.Stock.AnyAsync(s => s.Id == id);
            return await this.McDBContext.MoocCourses.AnyAsync(c => c.Title == title);
        }


    }
}