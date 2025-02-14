﻿using Microsoft.AspNetCore.Hosting;
using Mooc.Application.Contracts.Course.Dto;

namespace Mooc.Application.Course
{
    public class TeacherService : CrudService<Teacher, TeacherReadDto, TeacherReadDto, long, FilterPagedResultRequestDto, CreateOrUpdateTeacherDto, CreateOrUpdateTeacherDto>,
        ITeacherService, ITransientDependency
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TeacherService(MoocDBContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(dbContext, mapper)
        {
            this._webHostEnvironment = webHostEnvironment;
        }

        protected override IQueryable<Teacher> CreateFilteredQuery(FilterPagedResultRequestDto input)
        {
            if (!string.IsNullOrEmpty(input.Filter))
            {
                return GetQueryable().Where(t => t.User.UserName.Contains(input.Filter));
            }

            return base.CreateFilteredQuery(input);
        }

        /// <summary>
        /// Create teacher
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public override async Task<TeacherReadDto> CreateAsync(CreateOrUpdateTeacherDto input)
        {
            //validate id 
            await ValidateIdAsync(input.UserId);

            //get user name and add to input
            var userNameForDisplay = await McDBContext.Users.Where(u => u.Id == input.UserId).Select(u => u.UserName).FirstOrDefaultAsync();
            input.DisplayName = userNameForDisplay;
            var teacherDto = await base.CreateAsync(input);
            return teacherDto;
        }

        /// <summary>
        /// GetByName
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<TeacherReadDto> GetTeacherByName(string input)
        {
            var user = await this.McDBContext.Users.FirstOrDefaultAsync(x => x.UserName == input);
            if (user == null)
            {
                throw new EntityNotFoundException($"Invalid username!");
            }
            var teacher = await this.GetQueryable().FirstOrDefaultAsync(t => t.UserId == user.Id);
            if (teacher == null)
            {
                throw new EntityNotFoundException($"No teacher created under this username");
            }
            var teacherOutput = this.Mapper.Map<TeacherReadDto>(teacher);
            return teacherOutput;
        }

        //Validate teacher
        protected virtual async Task ValidateIdAsync(long userId)
        {
            var user = await this.McDBContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new EntityNotFoundException($"The user with {userId} cannot be found in the database, teacher role cannot be assigned.");
            }
        }

    }
}
