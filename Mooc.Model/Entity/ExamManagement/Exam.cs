namespace Mooc.Model.Entity;

public class Exam : BaseEntityWithAudit
{
  public long? CourseId { get; set; }
  
  public string ExamTitle { get; set; }

  public int? Remark { get; set; }

  public int ExaminationTime { get; set; }

  public QuestionUpload AutoOrManual { get; set; }

  public int TotalScore { get; set; }

  public int TimePeriod { get; set; }

  // Navigation property:
  public User? CreatedByUser { get; set; }

  public virtual User? UpdatedByUser { get; set; }

  public virtual ICollection<ExamQuestion>? ExamQuestion { get; set; }

  public ExamPublish? ExamPublish { get; set; }

  public CourseInstance? CourseInstance { get; set; }
}