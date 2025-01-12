
namespace Mooc.Application.Contracts.Course.Dto;
public class ReadSessionDto : BaseEntityDto
{
  // Basic Fields
  public string Title { get; set; } = string.Empty;  
  public string Description { get; set; } = string.Empty;  
  public int Order { get; set; }

  // Foreign Key 
  public long CreatedByUserId { get; set; }  
  public long? UpdatedByUserId { get; set; }  
  public long CourseInstanceId { get; set; }

  // Timestamp 
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  // Media-related information
  public int MediaCount { get; set; } 
  public bool HasMedia { get; set; }
    public List<MediaDto> Media { get; set; }
}