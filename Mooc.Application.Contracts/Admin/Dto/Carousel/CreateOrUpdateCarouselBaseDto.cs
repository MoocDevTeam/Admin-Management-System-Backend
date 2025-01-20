namespace Mooc.Application.Contracts.Admin;

public class CreateOrUpdateCarouselBaseDto
{
    public string Title { get; set; } = "Untitled";
    public string? ImageUrl { get; set; }
    public string? RedirectUrl { get; set; }
    public bool IsActive { get; set; } = false;
    public int Position { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
