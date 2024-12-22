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

        public MoocCourseService(MoocDBContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(dbContext, mapper)
        {
            this._webHostEnvironment = webHostEnvironment;
        }

        // 创建带过滤条件的查询
        protected override IQueryable<MoocCourse> CreateFilteredQuery(FilterPagedResultRequestDto input)
        {
            if (!string.IsNullOrEmpty(input.Filter))
            {
                return GetQueryable().Where(x => x.Title.Contains(input.Filter) || x.CourseCode.Contains(input.Filter));
            }
            return base.CreateFilteredQuery(input);
        }

        // 创建新课程
        public override async Task<CourseDto> CreateAsync(CreateCourseDto input)
        {
            await ValidateCourseCodeAsync(input.Title, 0);

            var courseDto = await base.CreateAsync(input);
            return courseDto;
        }


        // 更新课程
        public override async Task<CourseDto> UpdateAsync(long id, UpdateCourseDto input)
        {
            // 更新前可以加入验证，确保没有其他课程使用相同的课程代码
            await ValidateCourseCodeAsync(input.CourseCode, id);
            return await base.UpdateAsync(id, input);
        }

        // 课程代码的唯一性验证
        protected virtual async Task ValidateCourseCodeAsync(string courseCode, long? expectedId = null)
        {
            var course = await this.GetQueryable().FirstOrDefaultAsync(c => c.CourseCode == courseCode);
            if (course != null && course.Id != expectedId)
            {
                throw new EntityAlreadyExistsException($"Course with code {courseCode} already exists", $"{courseCode} already exists");
            }
        }

        // 获取课程通过课程代码
        public async Task<MoocCourse> GetByCourseNameAsync(string courseName)
        {
            var course = await this.McDBContext.MoocCourses.FirstOrDefaultAsync(x => x.Title == courseName);
            // if (course == null)
            //     return null;
            // CourseDto courseOutput = _mapper.Map<CourseDto>(course);
            return course;
        }


        Task<CourseDto> IMoocCourseService.GetByCourseNameAsync(string courseName)
        {
            throw new NotImplementedException();
        }

        // public async Task<List<CourseDto>> GetAllAsync()
        // {
        //     var courses = await this.McDBContext.MoocCourses.ToListAsync();


        //     if (courses.Count == 0)
        //         return new List<CourseDto>(); // 如果没有课程，返回空列表
        //     var courseOutput = this._mapper.Map<List<CourseDto>>(courses);
        //     return courseOutput;

        // }


    }
}