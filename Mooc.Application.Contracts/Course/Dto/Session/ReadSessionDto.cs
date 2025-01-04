
namespace Mooc.Application.Contracts.Course.Dto;
public class ReadSessionDto : BaseEntityDto
{
  // 基础字段
  public string Title { get; set; } = string.Empty;  
  public string Description { get; set; } = string.Empty;  
  public int Order { get; set; } 

  // 外键字段
  public long CreatedByUserId { get; set; }  
  public long? UpdatedByUserId { get; set; }  
  public long CourseInstanceId { get; set; }

  // 时间戳字段
  public DateTime UpdatedAt { get; set; } 

  // 可选的媒体相关信息
  public int MediaCount { get; set; } // 媒体数量
  public bool HasMedia { get; set; } // 是否存在媒体内容
}