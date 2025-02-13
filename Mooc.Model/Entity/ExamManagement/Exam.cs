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
  public virtual  User? CreatedByUser { get; set; }

  public virtual User? UpdatedByUser { get; set; }

  public virtual List<ExamQuestion>? ExamQuestions { get; set; }

  public virtual  ExamPublish? ExamPublish { get; set; }

  public virtual CourseInstance? CourseInstance { get; set; }
}