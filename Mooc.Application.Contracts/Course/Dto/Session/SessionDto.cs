
namespace Mooc.Application.Contracts.Course.Dto;
public class SessionDto : BaseEntityDto 
{
  // Basic Fields
  [Required(ErrorMessage = "Title is required")]
  [StringLength(50, ErrorMessage = "Title must be less than 50 characters")]
  public string Title { get; set; } = string.Empty;
  [Required(ErrorMessage = "Description is required")]
  [StringLength(255, ErrorMessage = "Description must be less than 255 characters")]
  public string Description { get; set; } = string.Empty;
  [Required(ErrorMessage = "Order is required")]
  public int Order { get; set; }

  // Foreign Key 
  [Required(ErrorMessage = "CreatedByUserId is required")]
  public long CreatedByUserId { get; set; } 
  public long? UpdatedByUserId { get; set; }
  [Required(ErrorMessage = "CourseInstanceId is required")]
  public long CourseInstanceId { get; set; }

  // Timestamp 
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; } 

  // Media Related Fields
  public int MediaCount { get; set; }
  public bool HasMedia { get; set; }
  }