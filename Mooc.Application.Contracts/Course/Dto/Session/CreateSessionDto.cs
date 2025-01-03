namespace Mooc.Application.Contracts.Course.Dto;
public class CreateSessionDto : BaseEntityDto {
  //Base Fields
  [Required(ErrorMessage = "Title is required")]
  [StringLength(50, ErrorMessage = "Title must be less than 100 characters")]
  public string Title { get; set; } = string.Empty;
  [Required(ErrorMessage = "Description is required")]
  [StringLength(255, ErrorMessage = "Description must be less than 255 characters")]
  public string Description { get; set; } = string.Empty;
  [Required(ErrorMessage = "Order is required")]
  public int Order { get; set; }

  // Foreign Key Fields
  [Required(ErrorMessage = "CreatedByUserId is required")]
  public long CreatedByUserId { get; set; }
  [Required(ErrorMessage = "CourseInstanceId is required")]
  public long CourseInstanceId { get; set; }
  // 时间戳字段会由后端系统自动更新，不需要提供
}