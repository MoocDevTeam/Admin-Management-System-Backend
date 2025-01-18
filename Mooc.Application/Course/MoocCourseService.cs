using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mooc.Application.Contracts.Course;

using Mooc.Core.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Mooc.Application.Course
{
    /// <summary>
    /// Service class for managing MoocCourse entities.
    /// Inherits from CrudService to provide basic CRUD operations.
    /// </summary>

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

        /// Creates a filtered query based on the provided input.
        /// Filters by Title or CourseCode if a filter is provided.
        protected override IQueryable<MoocCourse> CreateFilteredQuery(FilterPagedResultRequestDto input)
        {
            if (!string.IsNullOrEmpty(input.Filter))
            {
                return GetQueryable().Where(x => x.Title.Contains(input.Filter) || x.CourseCode.Contains(input.Filter));
            }
            return base.CreateFilteredQuery(input);
        }

        /// Creates a new course.
        public override async Task<CourseDto> CreateAsync(CreateCourseDto input)
        {

            var courseDto = await base.CreateAsync(input);
            return courseDto;
        }

        /// Updates an existing course.
        public override async Task<CourseDto> UpdateAsync(long id, UpdateCourseDto input)
        {
            await ValidateCourseNameAsync(input.CourseCode, id);
            return await base.UpdateAsync(id, input);
        }


        /// Validates that the course name is unique.
        /// Throws an exception if a course with the same name already exists.
        protected virtual async Task ValidateCourseNameAsync(string courseName, long? expectedId = null)
        {
            var course = await this.GetQueryable().FirstOrDefaultAsync(c => c.Title == courseName);
            if (course != null)
            {
                throw new EntityAlreadyExistsException($"Course with code {courseName} already exists", $"{courseName} already exists");
            }
        }

        /// Fetches a course by its name, including related data (Category, CourseInstances, TeacherCourseInstances, Teacher).
        public async Task<CourseDto> GetByCourseNameAsync(string courseName)
        {
            var course = await this.McDBContext.MoocCourses
                .Include(c => c.Category)
                .Include(c => c.CourseInstances)
                .ThenInclude(ci => ci.TeacherCourseInstances)
                        .ThenInclude(tci => tci.Teacher)
                .Include(c => c.CourseInstances)
                    .ThenInclude(ci => ci.Sessions)
                        .ThenInclude(s => s.Sessionmedia)
                .Include(c => c.CourseInstances)
                    .ThenInclude(ci => ci.Enrollment)
                .FirstOrDefaultAsync(c => c.Title.ToLower() == courseName.ToLower());

            if (course == null)
            {
                // Handle course not found (maybe throw an exception)
                return null;
            }

            // 2. Map to DTO and populate additional properties
            var courseDto = this._mapper.Map<CourseDto>(course);
            courseDto.CategoryName = course.Category?.CategoryName;

            var courseOutput = this.Mapper.Map<CourseDto>(course);
            courseOutput.CategoryName = course.Category?.CategoryName;

            return courseOutput;
        }

        /// Gets a course by its ID, including related data (Category, CourseInstances, TeacherCourseInstances, Teacher).
        public async override Task<CourseDto> GetAsync(long id)
        {
            // 1. Fetch the course with related data
            var course = await this.McDBContext.MoocCourses
                .Include(c => c.Category)
                .Include(c => c.CourseInstances)
                    .ThenInclude(ci => ci.TeacherCourseInstances)
                        .ThenInclude(tci => tci.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);


            if (course == null)
            {
                // Handle course not found (maybe throw an exception)
                return null;
            }

            // 2. Map to DTO and populate additional properties
            var courseDto = this._mapper.Map<CourseDto>(course);
            courseDto.CategoryName = course.Category?.CategoryName;

            return courseDto;
        }

        /// Gets all courses, including related Category.
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

        /// Checks if a course with the given title exists.
        public async Task<bool> CourseExist(string title)
        {
            // return _context.Stock.AnyAsync(s => s.Id == id);
            return await this.McDBContext.MoocCourses.AnyAsync(c => c.Title == title);
        }


        /// Gets all course instances for a given course ID.
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

        /// Gets a paged list of courses.
        public async Task<PagedResultDto<CourseDto>> GetListAsync(FilterPagedResultRequestDto input)
        {
            return await base.GetListAsync(input);
        }



    }
}