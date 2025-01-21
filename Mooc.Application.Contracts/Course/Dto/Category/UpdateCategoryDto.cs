using Mooc.Model.Entity;
using System.Text.Json.Serialization;

namespace Mooc.Application.Contracts.Course.Dto;

public class UpdateCategoryDto : BaseEntityDto
{
    public string CategoryName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public long? ParentId { get; set; }
    
    [JsonIgnore]
    public long? UpdatedByUserId { get; set; } = 1;
  
    [JsonIgnore]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}
