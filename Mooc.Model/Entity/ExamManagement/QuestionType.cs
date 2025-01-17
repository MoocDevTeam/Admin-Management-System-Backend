namespace Mooc.Model.Entity;

public class QuestionType : BaseEntity
{
    public DateTime? CreatedAt { get; set; }

    public string? QuestionTypeName { get; set; }

    // Navigation property:
    public virtual ICollection<ChoiceQuestion>? ChoiceQuestions { get; set; }

    public virtual ICollection<JudgementQuestion>? JudgementQuestions { get; set; }

    public virtual ICollection<ShortAnsQuestion>? ShortAnsQuestions { get; set; }
}