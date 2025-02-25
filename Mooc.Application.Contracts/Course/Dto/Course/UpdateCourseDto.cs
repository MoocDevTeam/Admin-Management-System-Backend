namespace Mooc.Application.Contracts.Course;

public class UpdateCourseDto : BaseEntityDto
{
    [Required(ErrorMessage = "Title is null")]
    [StringLength(100, ErrorMessage = "Title must be less than 100 characters")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "CourseCode is null")]
    [StringLength(100, ErrorMessage = "CourseCode must be less than 100 characters")]
    public string CourseCode { get; set; } = string.Empty;
    public string? CoverImage { get; set; }
    [Required(ErrorMessage = "Description is null")]
    [StringLength(100, ErrorMessage = "Description must be less than 100 characters")]
    public string Description { get; set; } = string.Empty;
    [Required(ErrorMessage = "CategoryId is null")]
    public long CategoryId { get; set; } = 1;

}