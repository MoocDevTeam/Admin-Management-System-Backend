namespace Mooc.Model.Entity;
public class ExamPublish:BaseEntity
{
  public int examId { get; set; }
  public DateTime publishedAt { get; set; }
  public int publishedByUserId { get; set; }
  public DateTime? closeAt { get; set; }
  public int courseInstanceId { get; set; }
}