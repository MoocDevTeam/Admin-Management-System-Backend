using Mooc.Model.Entity;

namespace Mooc.Application.Contracts.Course.Dto;

public class CategoryDto: BaseEntityDto
{
    public string CategoryName { get; set; }= string.Empty;
    public string Description { get; set; } = string.Empty;
   
    public string? IconUrl { get; set; }
    public long? ParentId { get; set; }

    public ICollection<CourseDto> Courses { get; set; } = new List<CourseDto>();
    public ICollection<CategoryDto> ChildrenCategories {  get; set; } = new List<CategoryDto>();
}
