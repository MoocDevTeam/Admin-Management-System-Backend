using Mooc.Model.Entity.ExamManagement;

namespace Mooc.Model.Entity;
public class ExamQuestion : BaseExam
{
  public long? ExamId { get; set; }

  public Exam? Exam { get; set; }

  public long? ChoiceQuestionId { get; set; }

  public long? JudgementQuestionId { get; set; }

  public long? ShortAnsQuestionId { get; set; }

  public int Marks { get; set; }

  public int QuestionOrder { get; set; }

  // public int QuestionTypeId { get; set; }


  // Navigation property:

  public User? CreatedByUser { get; set; }

  public virtual ICollection<User>? UpdatedByUsers { get; set; }


    /* we can choose either have 3 columns(ChoiceQuestionId, JudgementQuestionId, ShortAnsQuestionId) or have 1 column(questionId)*/
  //public long QuestionId { get; set; }   // the foreign key will be removed if choose 1 column(questionId) and we will use a fake foreign key  Discriminator: 

  //public string? QuestionType { get; set; } // Discriminator: "Choice", "Judgement", or "ShortAnswer"

  public ChoiceQuestion? ChoiceQuestion { get; set; }

  public JudgementQuestion? JudgementQuestion { get; set; }

  public ShortAnsQuestion? ShortAnsQuestion { get; set; }
}