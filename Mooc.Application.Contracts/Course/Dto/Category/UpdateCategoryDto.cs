using Mooc.Model.Entity;

namespace Mooc.Application.Contracts.Course.Dto;

public class UpdateCategoryDto : BaseEntityDto
{
    public string CategoryName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public long? ParentId { get; set; }
}
