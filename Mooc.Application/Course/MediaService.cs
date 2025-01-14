using Microsoft.AspNetCore.Hosting;
using Mooc.Application.Contracts.Course.Dto;
using Mooc.Shared.Enum;

namespace Mooc.Application.Course
{
  public class MediaService : CrudService<Media, ReadMediaDto, ReadMediaDto, long, FilterPagedResultRequestDto, CreateMediaDto, UpdateMediaDto>,
  IMediaService, ITransientDependency
  {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;
    public MediaService(
      MoocDBContext dbContext,
      IMapper mapper,
      IWebHostEnvironment webHostEnvironment)
      : base(dbContext, mapper)
    {
      this._webHostEnvironment = webHostEnvironment;
      this._mapper = mapper;
    }

    // Override MapToEntity
    protected override Media MapToEntity(CreateMediaDto input)
    {
      var entity = base.MapToEntity(input);
      entity.CreatedByUserId = 1;//---> need a method (getCurrentUserId)
      entity.UpdatedByUserId = 1;//---> need a method (getCurrentUserId)
      entity.ApprovalStatus = MediaApprovalStatus.Pending;
      entity.CreatedAt = DateTime.Now; // write here or profile
      entity.UpdatedAt = DateTime.Now;
      ValidateFileExtensionAndSetType(input.FilePath, entity); // Synchronized verification
      SetIdForLong(entity);
      return entity;
    }

    // Set the fileType by file path
    private void ValidateFileExtensionAndSetType(string filePath, Media entity)
    {
      var fileExtension = Path.GetExtension(filePath)?.ToLower();

      if (fileExtension != ".pdf" && fileExtension != ".mp4")
      {
        throw new InvalidOperationException("Invalid file type. Only PDF and MP4 files are allowed.");
      }

      entity.FileType = fileExtension == ".pdf" ? MediaFileType.Pdf : MediaFileType.Video;
    }

    // Verify that the SessionId exist
    private async Task ValidateSessionAsync(long seesionId)
    {
      var sessionExists = await McDBContext.Session.AnyAsync(x => x.Id == seesionId);
      if (!sessionExists)
      {
        throw new EntityNotFoundException($"Session with ID {seesionId} not found. Please check the input");
      }
    }

    //Verify that the MediaId exist
    protected virtual async Task ValidateMediaIdAsync(long mediaId)
    {
      var mediaExist = await McDBContext.Media.AnyAsync(x => x.Id == mediaId);
      if (!mediaExist)
      {
        throw new EntityNotFoundException($"Media with ID '{mediaExist}' does not exist. Please check the input.");
      }
    }

    // Create Media
    public override async Task<ReadMediaDto> CreateAsync(CreateMediaDto input)
    {
      await ValidateSessionAsync(input.SessionId); 
      var createMediaDto = await base.CreateAsync(input);
      return createMediaDto;
    }

    // Update Media
    public override async Task<ReadMediaDto> UpdateAsync(long id, UpdateMediaDto input)
    {
      ValidateMediaIdAsync(id);
      var entity = await GetEntityByIdAsync(id);
      if (input.SessionId.HasValue)
      {
        await ValidateSessionAsync(input.SessionId.Value);
        entity.SessionId = input.SessionId.Value; 
      }
      return await base.UpdateAsync(id, input);
    }

    // Get by page
    public async Task<PagedResultDto<ReadMediaDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
      return await base.GetListAsync(input);
    }

    // Get Media by Id
    public async Task<ReadMediaDto> GetAsync(long id)
    {
      await ValidateMediaIdAsync(id);
      return await base.GetAsync(id);
    }

    // Get Media by filename
    public async Task<IEnumerable<ReadMediaDto>> GetMediaByName(string mediaName)
    {
      var mediaList = await McDBContext.Media
        .Where(m => m.FileName.Contains(mediaName)).ToListAsync();

      if (!mediaList.Any())
      {
        throw new EntityNotFoundException("No sessions found with the specified title.");
      }
      return mediaList.Select(m => MapToGetOutputDto(m));
    }

    // Get by sessionId
    public async Task<IEnumerable<ReadMediaDto>> GetMeidaBySessionId(long seesionId)
    {
      await ValidateSessionAsync(seesionId);

      var mediaRecords = await this.McDBContext.Media.Where(m => m.SessionId == seesionId).ToListAsync();

      var mediaForSession = new List<ReadMediaDto>();

      foreach (var media in mediaRecords)
      {
        var mediaDto = MapToGetOutputDto(media); 
        mediaForSession.Add(mediaDto);
      }
      return mediaForSession;
    }

    //Delete
    public async Task DeleteAsync(long id)
    {
      await ValidateMediaIdAsync(id);
      await base.DeleteAsync(id);
    }
  }
}