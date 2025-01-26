namespace Mooc.Model.Entity;

public class ExamQuestion : BaseEntityWithAudit
{ 
  public long? ExamId { get; set; }

  public virtual Exam? Exam { get; set; }

  public long? ChoiceQuestionId { get; set; }

  public long? JudgementQuestionId { get; set; }

  public long? ShortAnsQuestionId { get; set; }

  public int Marks { get; set; }

  public int QuestionOrder { get; set; }

  // public int QuestionTypeId { get; set; }


  // Navigation property:

  public virtual User? CreatedByUser { get; set; }

  public virtual User? UpdatedByUser { get; set; }


    /* we can choose either have 3 columns(ChoiceQuestionId, JudgementQuestionId, ShortAnsQuestionId) or have 1 column(questionId)*/
  //public long QuestionId { get; set; }   // the foreign key will be removed if choose 1 column(questionId) and we will use a fake foreign key  Discriminator: 

  //public string? QuestionType { get; set; } // Discriminator: "Choice", "Judgement", or "ShortAnswer"

  public virtual ChoiceQuestion? ChoiceQuestion { get; set; }

  public virtual JudgementQuestion? JudgementQuestion { get; set; }

  public virtual ShortAnsQuestion? ShortAnsQuestion { get; set; }
}