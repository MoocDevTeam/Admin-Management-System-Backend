using Mooc.Shared.Enum;

namespace Mooc.Application.Contracts.Course.Dto;
public class ReadMediaDto : BaseEntityDto
{
  //Basic fields
  public MediaFileType FileType { get; set; }
  public string FileName { get; set; }
  public string FilePath { get; set; }
  public string ThumbnailPath { get; set; }
  public MediaApprovalStatus ApprovalStatus { get; set; }

  // Foreign Key
  public long SessionId { get; set; }
}