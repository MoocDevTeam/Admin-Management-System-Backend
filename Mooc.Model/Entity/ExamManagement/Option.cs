namespace Mooc.Model.Entity;

public class Option : BaseEntity
{
    public long ChoiceQuestionId { get; set; }
    public long OptionOrder { get; set; }
    public string OptionValue { get; set; }
    public virtual ICollection<ChoiceQuestion> ChoiceQuestions { get; set; }
}