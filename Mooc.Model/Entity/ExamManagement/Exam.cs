namespace Mooc.Model.Entity;
public class Exam:BaseEntity
{
  public int courseId { get; set; }
  public string examTitle { get; set; }
  public int? remark { get; set; }
  public int examinationTime { get; set; }
  public DateTime createdAt { get; set; }
  public int createdByUserId { get; set; }
  public int? updatedByUserId { get; set; }
  public DateTime? updatedAt { get; set; }

  public QuestionUpload autoOrManual { get; set; }
  public int totalScore { get; set; }
  public int timePeriod { get; set; }
}