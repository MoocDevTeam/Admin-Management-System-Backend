namespace Mooc.Model.Entity;
public class ExamQuestion:BaseEntity
{
  public int examId { get; set; }
  public int questionId { get; set; }
  public int marks { get; set; }
  public int questionOrder { get; set; }
  public DateTime createdAt { get; set; }
  public int createdByUserId { get; set; }
  public int? updatedByUserId { get; set; }
  public DateTime? updatedAt { get; set; }
  public int questionTypeId { get; set; }
}