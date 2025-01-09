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

    // Verify the file extension and set the file type
    private void ValidateFileExtensionAndSetType(string filePath, Media entity)
    {
      var fileExtension = Path.GetExtension(filePath)?.ToLower();

      if (fileExtension != ".pdf" && fileExtension != ".mp4")
      {
        throw new InvalidOperationException("Invalid file type. Only PDF and MP4 files are allowed.");
      }

      entity.FileType = fileExtension == ".pdf" ? MediaFileType.Pdf : MediaFileType.Video;
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

    // Verify that the SessionId exists
    private async Task ValidateSessionAsync(long seesionId)
    {
      var sessionExists = await McDBContext.Session.AnyAsync(x => x.Id == seesionId);
      if (!sessionExists)
      {
        throw new EntityNotFoundException($"Session with ID {seesionId} not found.");
      }
    }

    // Create Media
    public override async Task<ReadMediaDto> CreateAsync(CreateMediaDto input)
    {
      await ValidateSessionAsync(input.SessionId);
      var createSMediaDto = await base.CreateAsync(input);
      return createSMediaDto;
    }

    // Update Media
    public override async Task<ReadMediaDto> UpdateAsync(long id, UpdateMediaDto input)
    {
      var entity = await GetEntityByIdAsync(id);
      // Update the entity's SessionId if it was entered, if not leave it as is.
      if (input.SessionId.HasValue)
      {
        entity.SessionId = input.SessionId.Value; 
      }
      entity.ApprovalStatus = MediaApprovalStatus.Pending;
      ValidateFileExtensionAndSetType(input.FilePath, entity);

      return await base.UpdateAsync(id, input);
    }

    // get by page
    public async Task<PagedResultDto<ReadMediaDto>> GetListAsync(FilterPagedResultRequestDto input)
    {
      return await base.GetListAsync(input);
    }

    // get by mediaId
    public async Task<ReadMediaDto> GetAsync(long id)
    {
      return await base.GetAsync(id);
    }

    // get by filename
    public async Task<IEnumerable<ReadMediaDto>> GetMediaByName(string mediaName)
    {
      var mediaList = await McDBContext.Media
                                        .Where(m => m.FileName.Contains(mediaName))  
                                        .ToListAsync();

      return mediaList.Select(m => MapToGetOutputDto(m));
      throw new NotImplementedException();
    }

    // get by sessionId
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

    //delete
    public async Task DeleteAsync(long id)
    {
      await base.DeleteAsync(id);
    }
  }
}