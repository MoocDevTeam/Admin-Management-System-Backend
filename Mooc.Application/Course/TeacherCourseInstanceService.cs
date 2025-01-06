using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Mooc.Application.Contracts.Course.Dto;
using Mooc.Model.Entity;
using System.Collections;
using System.Runtime.InteropServices;

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

        //Create TeacherCourseInstance (Assign teacher to a specific course instance)
        public override async Task<TeacherCourseInstanceReadDto> CreateAsync(TeacherCourseInstanceCreateOrUpdateDto input)
        {
            // Validate if the CourseInstance exists
            await ValidateCourseInstanceId(input.CourseInstanceId);

            // Validate if the Teacher exists and avoid duplicate assignment to the same CourseInstance
            await ValidateTeacherId(input.TeacherId, input.CourseInstanceId);

            // Proceed with the creation using the base method
            var teacherCourseInstanceDto = await base.CreateAsync(input);
            return teacherCourseInstanceDto;
        }

        //update This is designed to change the autherization type for a teacher under a course instance 
        public override async Task<TeacherCourseInstanceReadDto> UpdateAsync(long id, TeacherCourseInstanceCreateOrUpdateDto input)
        {
            await ValidateTeacherCourseInstanceId(id);
            return await base.UpdateAsync(id, input);
        }

        //Get list of CourseInstances assigned to a specific teacher
        public async Task<List<CourseInstanceDto>> GetCourseInstanceListAsync(long id)
        {
            var courseInstanceIds = await McDBContext.TeacherCourseInstances
                .Where(tc => tc.TeacherId == id)
                .Select(tc => tc.CourseInstanceId)
                .ToListAsync();

            if (courseInstanceIds == null || !courseInstanceIds.Any())
            {
                throw new EntityNotFoundException("No course instance found for this teacher.");
            }
            var courseInstances = await McDBContext.CourseInstances
                .Where(ci => courseInstanceIds.Contains(ci.Id))
                .ToListAsync();

            return Mapper.Map<List<CourseInstanceDto>>(courseInstances);

        }

        /// <summary>
        /// Methods used for CRUD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        //validate TeacherCourseInstanceId
        protected virtual async Task ValidateTeacherCourseInstanceId(long id)
        {
            var teacherCourseInstance = await this.McDBContext.TeacherCourseInstances.FirstOrDefaultAsync(x => x.Id == id);

            if (teacherCourseInstance == null)
            {
                throw new EntityNotFoundException($"No course instance found with id {id}");
            }
        }

        // Method to validate Teacher existence and prevent duplicate assignment
        private async Task ValidateTeacherId(long teacherId, long courseInstanceId)
        {
            // Check if the teacher exists
            var teacher = await this.McDBContext.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacher == null)
            {
                throw new EntityNotFoundException($"Teacher with ID {teacherId} does not exist.");
            }

            // Check if the teacher is already assigned to the specific course instance
            var isAssigned = await this.McDBContext.TeacherCourseInstances
                .AnyAsync(tc => tc.TeacherId == teacherId && tc.CourseInstanceId == courseInstanceId);
            if (isAssigned)
            {
                throw new InvalidOperationException($"Teacher with ID {teacherId} is already assigned to Course Instance {courseInstanceId}.");
            }
        }

        //Check if the course instance exists
        protected virtual async Task ValidateCourseInstanceId(long id)
        {
            var courseInstance = await this.McDBContext.CourseInstances.FirstOrDefaultAsync(x => x.Id == id);
            if (courseInstance == null)
            {
                throw new EntityNotFoundException($"The course instance with id {id} dose not exist, you cannot assign a teacher to a course instance that dose not exist.");
            }
        }

        public async Task<TeacherCourseInstanceReadDto> GetTeacherCourseInstanceById(long id)
        {
            await ValidateTeacherCourseInstanceId(id);
            return await base.GetAsync(id);
        }

        //Override MapToEntity
        protected override TeacherCourseInstance MapToEntity(TeacherCourseInstanceCreateOrUpdateDto input)
        {
            var entity = base.MapToEntity(input);
            entity.CreatedByUserId = 1;//---> need a method (getCurrentUserId)get your jwt read your token to get the specific id and store 
            entity.UpdatedByUserId = 1;
            SetIdForLong(entity);
            return entity;
        }

    
    }
}
 