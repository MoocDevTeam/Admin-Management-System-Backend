using Mooc.Model.Entity;

namespace Mooc.Application.Contracts.Course.Dto.Category;

public class UpdateCategoryDto : BaseEntityWithAudit
{
    public string CategoryName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public long? ParentId { get; set; }
}
