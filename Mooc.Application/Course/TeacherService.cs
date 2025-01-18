using Microsoft.AspNetCore.Hosting;
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

        //Create teacher
        public override async Task<TeacherReadDto> CreateAsync(CreateOrUpdateTeacherDto input)
        {
            if (input == null)
            { 
                throw new ArgumentNullException(nameof(input));
            }
            await ValidateIdAsync(input.UserId);
            var teacherDto = await base.CreateAsync(input);
            return teacherDto;
        }

        //Update
        public override async Task<TeacherReadDto> UpdateAsync(long id, CreateOrUpdateTeacherDto input)
        {
            await ValidateIdAsync(id);
            return await base.UpdateAsync(id, input);
        }

        //GetByName
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
        //GetAsync
        //public new async Task<TeacherReadDto> GetAsync(long id)
        //{
        //    await this.ValidateIdAsync(id);
        //    return await base.GetAsync(id);
        //}

        //Validate teacher
        protected virtual async Task ValidateIdAsync(long userId)
        {
            var user = await this.McDBContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new EntityNotFoundException($"The user with {userId} cannot be found in the database, teacher role cannot be assigned.");
            }
        }

        //Override MapToEntity
        //protected override Teacher MapToEntity(CreateOrUpdateTeacherDto input)
        //{
        //    var entity = base.MapToEntity(input);
        //    entity.CreatedByUserId = 1;//---> need a method (getCurrentUserId)get your jwt read your token to get the specific id and store 
        //    entity.UpdatedByUserId = 1;
        //    SetIdForLong(entity);
        //    return entity;
        //}
    }
}
