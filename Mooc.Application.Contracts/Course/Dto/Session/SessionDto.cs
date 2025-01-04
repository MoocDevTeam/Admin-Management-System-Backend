
namespace Mooc.Application.Contracts.Course.Dto;
public class SessionDto : BaseEntityDto 
{
  // 基础字段
  [Required(ErrorMessage = "Title is required")]
  [StringLength(50, ErrorMessage = "Title must be less than 100 characters")]
  public string Title { get; set; } = string.Empty;
  [Required(ErrorMessage = "Description is required")]
  [StringLength(255, ErrorMessage = "Description must be less than 255 characters")]
  public string Description { get; set; } = string.Empty;
  [Required(ErrorMessage = "Order is required")]
  public int Order { get; set; }

  // 外键字段
  [Required(ErrorMessage = "CreatedByUserId is required")]
  public long CreatedByUserId { get; set; } 
  public long? UpdatedByUserId { get; set; }
  [Required(ErrorMessage = "CourseInstanceId is required")]
  public long CourseInstanceId { get; set; }

  // 时间戳字段
  public DateTime UpdatedAt { get; set; } 

  // 媒体相关字段
  public int MediaCount { get; set; }
  public bool HasMedia { get; set; }

}