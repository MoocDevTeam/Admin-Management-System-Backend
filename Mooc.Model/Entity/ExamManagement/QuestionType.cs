namespace Mooc.Model.Entity.ExamManagement;

public class QuestionType : BaseEntity
{
    public string? QuestionTypeName { get; set; }

    public ICollection<ChoiceQuestion>? ChoiceQuestions { get; set; }

    public ICollection<JudgementQuestion>? JudgementQuestions { get; set; }

    public ICollection<ShortAnsQuestion>? ShortAnsQuestions { get; set; }
}