using System.ComponentModel.DataAnnotations;
namespace Mooc.Application.Contracts.Admin;

public class CreateOrUpdateCarouselBaseDto
{
    [StringLength(20, ErrorMessage = "Title cannot exceed 20 characters.")]
    public string Title { get; set; } = "Untitled";

    [Required(ErrorMessage = "ImageUrl is required.")]
    [RegularExpression(@"^(https?|ftp)://[^\s/$.?#].[^\s]*$", ErrorMessage = "Invalid URL format.")]
    public string? ImageUrl { get; set; }

    public string? RedirectUrl { get; set; }

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Position is required.")]
    [Range(1, 5, ErrorMessage = "Position must be between 1 and 5.")]
    public int? Position { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}
