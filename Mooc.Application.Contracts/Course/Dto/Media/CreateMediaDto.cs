namespace Mooc.Application.Contracts.Course.Dto;
using Mooc.Shared.Enum;

public class CreateMediaDto : BaseEntityDto
{
  public MediaFileType FileType { get; set; }
  public MediaApprovalStatus ApprovalStatus { get; set; }
  //Base Fields
  // [Required(ErrorMessage = "FileName is required")]
  // [StringLength(255, ErrorMessage = "FileName must be less than 255 characters")]
  public string FileName { get; set; }
  // [Required(ErrorMessage = "FilePath is required")]
  // [RegularExpression(@"^.+\.(pdf|mp4)$", ErrorMessage = "Invalid file extension. Allowed extensions are .pdf and .mp4.")]
  public string FilePath { get; set; }
  // [Required(ErrorMessage = "ThumbnailPath is required")]
  public string ThumbnailPath { get; set; }

  // Foreign Key Fields
  // [Required(ErrorMessage = "SessionId is required")]
  public long SessionId { get; set; }

  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public long CreatedByUserId { get; set; }
  public long UpdatedByUserId { get; set; }
}