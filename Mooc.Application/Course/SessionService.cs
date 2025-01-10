using Microsoft.AspNetCore.Hosting;
using Mooc.Application.Contracts.Course.Dto;
using Mooc.Shared.Enum;

namespace Mooc.Application.Course
{
  public class SessionService : CrudService<Session, ReadSessionDto, ReadSessionDto, long, FilterPagedResultRequestDto, CreateOrUpdateSessionDto, CreateOrUpdateSessionDto>,ISessionService, ITransientDependency
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

    // Verify that the CourseInstanceId exists
    private async Task ValidateCourseInstanceAsync(long courseInstanceId)
    {
      var courseInstanceExists = await McDBContext.CourseInstances.AnyAsync(x => x.Id == courseInstanceId);
      if (!courseInstanceExists)
      {
        throw new EntityNotFoundException($"CourseInstance with ID {courseInstanceId} not found.");
      }
    }

    //Validate seesion with SessionId
    protected virtual async Task ValidateSessionIdAsync(long sessionId)
    {
      var user = await this.McDBContext.Session.FirstOrDefaultAsync(x => x.Id == sessionId);
      if (user == null)
      {
        throw new EntityNotFoundException($"Session with ID '{sessionId}' does not exist. Please check the input.");
      }
    }

    //Adds media info to the given session 
    private async Task AddMediaInfoToSessionDto(ReadSessionDto sessionMeidaDto, long sessionId)
    {
      // Query the number of Media associated with the session
      var mediaCount = await this.McDBContext.Media
          .Where(m => m.SessionId == sessionId)
          .CountAsync();

      var pendingCount = await this.McDBContext.Media
          .Where(m => m.SessionId == sessionId && m.ApprovalStatus == MediaApprovalStatus.Pending)
          .CountAsync();

      var approvedCount = await this.McDBContext.Media
          .Where(m => m.SessionId == sessionId && m.ApprovalStatus == MediaApprovalStatus.Approved)
          .CountAsync();

      var rejectedCount = await this.McDBContext.Media
          .Where(m => m.SessionId == sessionId && m.ApprovalStatus == MediaApprovalStatus.Rejected)
          .CountAsync();

      var mediaDetails = await this.McDBContext.Media
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
    public override async Task<ReadSessionDto> CreateAsync(CreateOrUpdateSessionDto input)
    {
      await ValidateCourseInstanceAsync(input.CourseInstanceId);
      var createSessionDto = await base.CreateAsync(input); 
      return createSessionDto;
    }

    //Update session
    public override async Task<ReadSessionDto> UpdateAsync(long id, CreateOrUpdateSessionDto input)
    {
      await ValidateSessionIdAsync(id);
      return await base.UpdateAsync(id, input);
    }

    //Get by page
    public async Task<PagedResultDto<ReadSessionDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
      return await base.GetListAsync(input);
    }

    //GetSessionById
    public override async Task<ReadSessionDto> GetAsync(long id)
    {
      await ValidateSessionIdAsync(id);
      var sigleSessionDetails = await base.GetAsync(id);
      await AddMediaInfoToSessionDto(sigleSessionDetails, id);
      return sigleSessionDetails;
    }

    /// <summary>
    /// Retrieves a session by its title and includes the number of associated media and media presence information.
    /// </summary>
    /// <param name="sessionName">The tit of the session to retrieve.</param>
    /// <returns>A ReadSessionDto containing the session details, including media count and presence indicator.</returns>
    /// <exception cref="EntityNotFoundException">Thrown if no session with the specified name is found.</exception>
    public async Task<ReadSessionDto> GetSessionByTitle(string sessionTitle)
    {
      // Verify if the session with the given name exists in the database.
      var session = await this.McDBContext.Session
          .FirstOrDefaultAsync(x => x.Title == sessionTitle);
      if (session == null)
      {
        throw new EntityNotFoundException($"Session with name '{sessionTitle}' does not exist.");
      }
      var singleSessionDetails = MapToGetOutputDto(session);
      await AddMediaInfoToSessionDto(singleSessionDetails, session.Id);

      return singleSessionDetails;
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
      var sessions = await this.McDBContext.Session
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

    // // Counts the existing sessions for the current CourseInstance and assigns an Order to the new session.
    // var order = await McDBContext.Session.CountAsync(x => x.CourseInstanceId == input.CourseInstanceId) + 1;

    // //get current order number
    // private async Task<int> getCurrentOrderNumber(long courseInstanceId)
    // {
    //   return await McDBContext.Session.CountAsync(x => x.Id == courseInstanceId);
    // }

  }
}