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

        /// <summary>
        /// Creates a filtered query based on the provided input.
        /// Filters by Title or CourseCode if a filter is provided.
        /// </summary>
        protected override IQueryable<MoocCourse> CreateFilteredQuery(FilterPagedResultRequestDto input)
        {
            if (!string.IsNullOrEmpty(input.Filter))
            {
                return GetQueryable().Where(x => x.Title.Contains(input.Filter) || x.CourseCode.Contains(input.Filter));
            }
            return base.CreateFilteredQuery(input);
        }

        /// <summary>
        /// Creates a new course.
        /// </summary>
        public override async Task<CourseDto> CreateAsync(CreateCourseDto input)
        {
            // Call the base method to create the course and return the DTO.
            var courseDto = await base.CreateAsync(input);
            return courseDto;
        }

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        public override async Task<CourseDto> UpdateAsync(long id, UpdateCourseDto input)
        {
            await ValidateCourseNameAsync(input.CourseCode, id);
            return await base.UpdateAsync(id, input);
        }

        /// <summary>
        /// Validates that the course name is unique.
        /// Throws an exception if a course with the same name already exists.
        /// </summary>
        protected virtual async Task ValidateCourseNameAsync(string courseName, long? expectedId = null)
        {
            // Check if another course with the same name exists.
            var course = await this.GetQueryable().FirstOrDefaultAsync(c => c.Title == courseName);
            if (course != null)
            {
                throw new EntityAlreadyExistsException($"Course with code {courseName} already exists", $"{courseName} already exists");
            }
        }

        /// <summary>
        /// Fetches a course by its name, including related data (Category, CourseInstances, TeacherCourseInstances, Teacher).
        /// </summary>        
        public async Task<CourseDto> GetByCourseNameAsync(string courseName)
        {
            // Fetch the course with related data based on the course name.
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

            // Map to DTO and populate additional properties
            var courseDto = this._mapper.Map<CourseDto>(course);
            courseDto.CategoryName = course.Category?.CategoryName;

            var courseOutput = this.Mapper.Map<CourseDto>(course);
            courseOutput.CategoryName = course.Category?.CategoryName;

            return courseOutput;
        }

        public async Task<CourseDto> GetByIdAsync(long id)
        {
            // Fetch the course with related data based on the course name.
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
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                // Handle course not found (maybe throw an exception)
                return null;
            }

            // Map to DTO and populate additional properties
            var courseDto = this._mapper.Map<CourseDto>(course);
            courseDto.CategoryName = course.Category?.CategoryName;

            var courseOutput = this.Mapper.Map<CourseDto>(course);
            courseOutput.CategoryName = course.Category?.CategoryName;

            return courseOutput;
        }

        /// <summary>
        /// Gets a course by its ID, including related data (Category, CourseInstances, TeacherCourseInstances, Teacher).
        /// </summary>
        public async override Task<CourseDto> GetAsync(long id)
        {
            // Fetch the course with related data
            var course = await this.McDBContext.MoocCourses
                .Include(c => c.Category)
                .Include(c => c.CourseInstances)
                    .ThenInclude(ci => ci.TeacherCourseInstances)
                        .ThenInclude(tci => tci.Teacher)
                .FirstOrDefaultAsync(c => c.Id == id);


            if (course == null)
            {
                // Return null if the course is not found.
                return null;
            }

            // Map the entity to a DTO and populate additional properties.
            var courseDto = this._mapper.Map<CourseDto>(course);
            courseDto.CategoryName = course.Category?.CategoryName;

            return courseDto;
        }

        /// <summary>
        /// Gets all courses, including related Category.
        /// </summary>
        public async Task<List<CourseListDto>> GetAllAsync()
        {
            // Fetch all courses with related data.
            var courses = await this.McDBContext.MoocCourses
                .Include(c => c.Category) // Include Category to load CategoryName
                .ToListAsync();
            if (courses.Count == 0)
                return new List<CourseListDto>();
            // Map the entities to DTOs and populate additional properties.
            var courseOutput = this._mapper.Map<List<CourseListDto>>(courses);

            courseOutput.ForEach(c => c.CategoryName = c.Category?.CategoryName);

            return courseOutput;
        }
        //         public async Task<List<CourseDto>> GetAllAsync()
        // {
        //     // Fetch all courses with related data.
        //     var courses = await this.McDBContext.MoocCourses
        //         .Include(c => c.Category) // Include Category to load CategoryName
        //         .Include(c => c.CourseInstances) // Include CourseInstances
        //         .ToListAsync();

        //     if (courses.Count == 0)
        //         return new List<CourseDto>();
        //     // Map the entities to DTOs and populate additional properties.
        //     var courseOutput = this._mapper.Map<List<CourseDto>>(courses);

        //     courseOutput.ForEach(c => c.CategoryName = c.Category?.CategoryName);

        //     return courseOutput;
        // }

        /// <summary>
        /// Checks if a course with the given title exists.
        /// </summary>
        public async Task<bool> CourseExist(string title)
        {
            // Check if a course with the given title exists in the database.
            return await this.McDBContext.MoocCourses.AnyAsync(c => c.Title == title);
        }


        /// <summary>
        /// Gets all course instances for a given course ID.
        /// </summary>
        public async Task<List<CourseInstanceDto>> GetCourseInstancesByCourseIdAsync(long courseId)
        {
            // Fetch all course instances for the given course ID.
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


        /// <summary>
        /// Gets a paged list of courses.
        /// </summary>
        public async Task<PagedResultDto<CourseDto>> GetListAsync(FilterPagedResultRequestDto input)
        {
            // Use the base method to fetch a paged list of courses.
            return await base.GetListAsync(input);
        }



    }
}