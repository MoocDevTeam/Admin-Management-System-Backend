using Microsoft.AspNetCore.Hosting;
using Mooc.Application.Contracts.Course.Dto;
using Microsoft.EntityFrameworkCore;
using Mooc.Model.Entity.Course;

namespace Mooc.Application.Course
{
  public class SessionService : CrudService<Session, ReadSessionDto, ReadSessionDto, long, FilterPagedResultRequestDto, CreateOrUpdateSessionDto, CreateOrUpdateSessionDto>,
ISessionService, ITransientDependency
  {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;
    public SessionService(
      MoocDBContext dbContext,
      IMapper mapper,
      IWebHostEnvironment webHostEnvironment)
      : base(dbContext, mapper)
    {
      this._webHostEnvironment = webHostEnvironment;
      this._mapper = mapper;
    }

    // Verify that the CourseInstanceId exists
    private async Task ValidateCourseInstanceAsync(long courseInstanceId)
    {
      var courseInstanceExists = await McDBContext.CourseInstances.AnyAsync(x => x.Id == courseInstanceId);
      if (!courseInstanceExists)
      {
        throw new EntityNotFoundException($"CourseInstance with ID {courseInstanceId} not found.");
      }
    }

    //Override MapToEntity
    protected override Session MapToEntity(CreateOrUpdateSessionDto input)
    {
      var entity = base.MapToEntity(input);
      entity.CreatedByUserId = 1;//---> need a method (getCurrentUserId)
      entity.UpdatedByUserId = 1;
      entity.Order = 10;//---> meed a method (getCurrentOrderNumber)
      entity.CreatedAt = DateTime.Now; // write here or profile
      entity.UpdatedAt = DateTime.Now;
      SetIdForLong(entity);
      return entity;
    }

    //Create session
    public override async Task<ReadSessionDto> CreateAsync(CreateOrUpdateSessionDto input)
    {
      await ValidateCourseInstanceAsync(input.CourseInstanceId);
      var createSessionDto = await base.CreateAsync(input); // Frontend -> Backend-> Database
      return createSessionDto;
    }

    // // Counts the existing sessions for the current CourseInstance and assigns an Order to the new session.
    // var order = await McDBContext.Session.CountAsync(x => x.CourseInstanceId == input.CourseInstanceId) + 1;


    // //get current order number
    // private async Task<int> getCurrentOrderNumber(long courseInstanceId)
    // {
    //   return await McDBContext.Session.CountAsync(x => x.Id == courseInstanceId);
    // }

    // GetAllSessionsByCourseInstanceId 
    Task<IEnumerable<ReadSessionDto>> ISessionService.GetAllSessionsByCourseInstanceId(long courseInstanceId)
    {
      throw new NotImplementedException();
    }

 
  }
}