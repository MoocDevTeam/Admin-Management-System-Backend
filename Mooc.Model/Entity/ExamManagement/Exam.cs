using Mooc.Model.Entity.ExamManagement;

namespace Mooc.Model.Entity;
public class Exam : BaseExam
{
/*  public long CourseId { get; set; }*/

  public string ExamTitle { get; set; }

  public int? Remark { get; set; }

  public int ExaminationTime { get; set; }

  public QuestionUpload AutoOrManual { get; set; }

  public int TotalScore { get; set; }

  public int TimePeriod { get; set; }

 // foreign key reference:
  public ICollection<ExamQuestion>? ExamQuestion { get; set; }

  public ExamPublish ExamPublish { get; set; }

/*  public Course Course { get; set; }*/
}