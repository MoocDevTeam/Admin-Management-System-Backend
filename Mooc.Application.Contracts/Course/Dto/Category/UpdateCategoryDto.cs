namespace Mooc.Application.Contracts.Course.Dto.Category;

public class UpdateCategoryDto
{
    public string CategoryName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public long? ParentId { get; set; }

    public long UpdatedByUserId { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
