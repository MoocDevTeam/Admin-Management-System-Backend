using Mooc.Model.Entity.ExamManagement;

namespace Mooc.Model.Entity;
public class ExamQuestion : BaseExam
{
  public long ExamId { get; set; }

  public long QuestionId { get; set; }

  public int Marks { get; set; }

  public int QuestionOrder { get; set; }

  public int QuestionTypeId { get; set; }

    // foreign key reference:
    public Exam? Exam { get; set; }
/*  public ChoiceQuestion ChoiceQuestion { get; set; }
    public JudgementQuestion JudgementQuestion { get; set; }
    public ShortAnsQuestion ShortAnsQuestion { get; set; }*/
}