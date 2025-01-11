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
            var course = await this.McDBContext.MoocCourses
            .Include(c => c.CourseInstances)
            .FirstOrDefaultAsync(x =>
                x.Title.ToLower() == courseName.ToLower());

            if (course == null)
                return null;

            var courseOutput = this.Mapper.Map<CourseDto>(course);
            return courseOutput;
        }




        public async Task<List<CourseDto>> GetAllAsync()
        {
            var courses = await this.McDBContext.MoocCourses
                .Include(c => c.Category) // Include Category to load CategoryName
                .Include(c => c.CourseInstances) // Include CourseInstances
                .ToListAsync();

            if (courses.Count == 0)
                return new List<CourseDto>();

            var courseOutput = this._mapper.Map<List<CourseDto>>(courses);

            courseOutput.ForEach(c => c.CategoryName = c.Category?.CategoryName);

            return courseOutput;
        }

        public async Task<bool> CourseExist(string title)
        {
            // return _context.Stock.AnyAsync(s => s.Id == id);
            return await this.McDBContext.MoocCourses.AnyAsync(c => c.Title == title);
        }
        public async Task<List<CourseInstanceDto>> GetCourseInstancesByCourseIdAsync(long courseId)
        {
            // Assuming you have a DbContext called _dbContext
            var courseInstances = await this.McDBContext.CourseInstances
                .Where(ci => ci.MoocCourseId == courseId) // Assuming CourseId is the foreign key
                .Select(ci => new CourseInstanceDto
                {
                    // Map properties from CourseInstance to CourseInstanceDto
                    Id = ci.Id,
                    // ... other properties
                })
                .ToListAsync();
            return courseInstances;
        }
        public async Task<PagedResultDto<CourseDto>> GetListAsync(FilterPagedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }



    }
}