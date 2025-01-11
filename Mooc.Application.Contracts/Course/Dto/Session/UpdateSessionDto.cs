namespace Mooc.Application.Contracts.Course.Dto;
public class UpdateSessionDto : BaseEntityDto
{
  //Base Fields
  [Required(ErrorMessage = "Title is required")]
  [StringLength(50, ErrorMessage = "Title must be less than 50 characters")]
  public string Title { get; set; } = string.Empty;
  [Required(ErrorMessage = "Description is required")]
  [StringLength(255, ErrorMessage = "Description must be less than 255 characters")]
  public string Description { get; set; } = string.Empty;

  // Nullable, allows CourseInstanceId not to be supplied if it is not modified.
  public long? CourseInstanceId { get; set; }
}