namespace Mooc.Model.Entity.ExamManagement;

public class QuestionType : BaseEntity
{
    public string? QuestionTypeName { get; set; }

    // Navigation property:
    public virtual ICollection<ChoiceQuestion>? ChoiceQuestions { get; set; }

    public virtual ICollection<JudgementQuestion>? JudgementQuestions { get; set; }

    public virtual ICollection<ShortAnsQuestion>? ShortAnsQuestions { get; set; }
}