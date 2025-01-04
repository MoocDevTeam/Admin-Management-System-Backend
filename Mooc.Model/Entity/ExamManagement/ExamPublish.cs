namespace Mooc.Model.Entity;
public class ExamPublish : BaseEntityWithAudit
{
  public long ExamId { get; set; }

  public DateTime? CloseAt { get; set; }

  public Exam? Exam { get; set; }
}