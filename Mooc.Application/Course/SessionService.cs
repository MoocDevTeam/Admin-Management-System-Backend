using Microsoft.AspNetCore.Hosting;
using Mooc.Application.Contracts.Course.Dto;
using Mooc.Shared.Enum;

namespace Mooc.Application.Course
{
  public class SessionService : CrudService<Session, ReadSessionDto, ReadSessionDto, long, FilterPagedResultRequestDto, CreateSessionDto, UpdateSessionDto>,ISessionService, ITransientDependency
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

    //Override MapToEntity
    protected override Session MapToEntity(CreateSessionDto input)
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

    // Verify that the CourseInstanceId exist
    private async Task ValidateCourseInstanceAsync(long courseInstanceId)
    {
      var courseInstanceExists = await McDBContext.CourseInstances.AnyAsync(x => x.Id == courseInstanceId);
      if (!courseInstanceExists)
      {
        throw new EntityNotFoundException($"CourseInstance with ID {courseInstanceId} not found. Please check the input.");
      }
    }

    //Verify that the SessionId exist
    protected virtual async Task ValidateSessionIdAsync(long sessionId)
    {
      var sessionExist = await McDBContext.Session.AnyAsync(x => x.Id == sessionId);
      if (!sessionExist)
      {
        throw new EntityNotFoundException($"Session with ID '{sessionId}' does not exist. Please check the input.");
      }
    }

    //Adds media info to the given session 
    private async Task AddMediaInfoToSessionDto(ReadSessionDto sessionMeidaDto, long sessionId)
    {
      // Query the number of Media associated with the session
      var mediaCount = await McDBContext.Media
          .Where(m => m.SessionId == sessionId)
          .CountAsync();

      var pendingCount = await McDBContext.Media
          .Where(m => m.SessionId == sessionId && m.ApprovalStatus == MediaApprovalStatus.Pending)
          .CountAsync();

      var approvedCount = await McDBContext.Media
          .Where(m => m.SessionId == sessionId && m.ApprovalStatus == MediaApprovalStatus.Approved)
          .CountAsync();


      var rejectedCount = await McDBContext.Media
          .Where(m => m.SessionId == sessionId && m.ApprovalStatus == MediaApprovalStatus.Rejected)
          .CountAsync();

      var mediaDetails = await McDBContext.Media
          .Where(m => m.SessionId == sessionId)
          .Select(m => new ReadMediaDto
          {
            Id = m.Id,
            FileName = m.FileName,
            FilePath = m.FilePath,
            ThumbnailPath = m.ThumbnailPath,
            ApprovalStatus = m.ApprovalStatus,
            FileType = m.FileType,
            SessionId = sessionId
          })
          .ToListAsync();

      // Set the properties on the sessionDto
      sessionMeidaDto.MediaCount = mediaCount;
      sessionMeidaDto.PendingCount = pendingCount;
      sessionMeidaDto.ApprovedCount = approvedCount;
      sessionMeidaDto.RejectedCount = rejectedCount;
      sessionMeidaDto.MediaFiles = mediaDetails;
    }

    //Create session
    public override async Task<ReadSessionDto> CreateAsync(CreateSessionDto input)
    {
      await ValidateCourseInstanceAsync(input.CourseInstanceId);
      var createSessionDto = await base.CreateAsync(input); 
      return createSessionDto;
    }

    //Update session
    public override async Task<ReadSessionDto> UpdateAsync(long id, UpdateSessionDto input)
    {
      await ValidateSessionIdAsync(id);
      var entity = await GetEntityByIdAsync(id);
      if (input.CourseInstanceId.HasValue) 
      {
        await ValidateCourseInstanceAsync(input.CourseInstanceId.Value);
        entity.CourseInstanceId = input.CourseInstanceId.Value;
      }
      return await base.UpdateAsync(id, input);
    }


    // Override query
    protected override IQueryable<Session> CreateFilteredQuery(FilterPagedResultRequestDto input)
    {
      if (!string.IsNullOrEmpty(input.Filter))
      {
        return GetQueryable().Where(s => s.Title.Contains(input.Filter));
      }

      return base.CreateFilteredQuery(input);
    }

    //Get by page
    public async Task<PagedResultDto<ReadSessionDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
      // Validate PageIndex and PageSize
      return await base.GetListAsync(input);
    }

    //Get Session by Id
    public override async Task<ReadSessionDto> GetAsync(long id)
    {
      await ValidateSessionIdAsync(id);
      var sigleSessionDto = await base.GetAsync(id);
      await AddMediaInfoToSessionDto(sigleSessionDto, id);
      return sigleSessionDto;
    }

    /// <summary>
    /// Retrieves sessions by title and includes associated media information such as media count and presence status.
    /// </summary>
    /// <param name="sessionTitle">The title or partial title of the session to search for.</param>
    /// <returns>A list of ReadSessionDto containing session details and media information.</returns>
    /// <exception cref="EntityNotFoundException">Thrown if no sessions are found with the specified title.</exception>
    public async Task<IEnumerable<ReadSessionDto>> GetSessionByTitle(string sessionTitle)
    {
      var sessionList = await McDBContext.Session
          .Where(s => s.Title.Contains(sessionTitle)).ToListAsync();

      if (!sessionList.Any())

      {
        throw new EntityNotFoundException("No sessions found with the specified title.");
      }

      var sessionDetailsList = new List<ReadSessionDto>();

      foreach (var session in sessionList)
      {
        var sessionDto = MapToGetOutputDto(session); 
        await AddMediaInfoToSessionDto(sessionDto, session.Id); 
        sessionDetailsList.Add(sessionDto);  
      }

      return sessionDetailsList;  
    }

    /// <summary>
    /// Retrieves all sessions related to a specific course instance, including media count and presence.
    /// </summary>
    /// <param name="courseInstanceId">The ID of the course instance.</param>
    /// <returns>A list of session DTOs with media count and presence details.</returns>
    public async Task<IEnumerable<ReadSessionDto>> GetAllSessionsByCourseInstanceId(long courseInstanceId)
    {
      await ValidateCourseInstanceAsync(courseInstanceId);

      // Fetch all sessions related to the course instance
      var sessions = await McDBContext.Session
          .Where(s => s.CourseInstanceId == courseInstanceId)
          .ToListAsync();

      var sessionsForCourseInstance = new List<ReadSessionDto>();

      // For each session, calculate media-related information
      foreach (var session in sessions)
      {
        var sessionDto = MapToGetOutputDto(session);
        await AddMediaInfoToSessionDto(sessionDto, session.Id);
        sessionsForCourseInstance.Add(sessionDto);
      }

      return sessionsForCourseInstance;
    }

    // Delete
    public async Task DeleteAsync(long id)
    {
      await ValidateSessionIdAsync(id);
      await base.DeleteAsync(id);
    }


  // // Counts the existing sessions for the current CourseInstance and assigns an Order to the new session.
  // var order = await McDBContext.Session.CountAsync(x => x.CourseInstanceId == input.CourseInstanceId) + 1;

  // //get current order number
  // private async Task<int> getCurrentOrderNumber(long courseInstanceId)
  // {
  //   return await McDBContext.Session.CountAsync(x => x.Id == courseInstanceId);
  // }

}
}