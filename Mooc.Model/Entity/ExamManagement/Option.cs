namespace Mooc.Model.Entity;

public class Option : BaseEntityWithAudit
{
    public long? ChoiceQuestionId { get; set; }
    public long? MultipleChoiceQuestionId { get; set; }

    public ChoiceQuestion? ChoiceQuestion { get; set; }
    public MultipleChoiceQuestion? MultipleChoiceQuestion { get; set; }

    public long OptionOrder { get; set; }

    public string? OptionValue { get; set; }

    public User? CreatedByUser { get; set; }

    public virtual User? UpdatedByUser { get; set; }

    public string? ErrorExplanation { get; set; }

    /*    public DateTime Field {  get; set; }*/

}