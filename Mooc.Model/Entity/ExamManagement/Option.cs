namespace Mooc.Model.Entity.ExamManagement;

public class Option : BaseEntityWithAudit
{
    public long ChoiceQuestionId { get; set; }

    public ChoiceQuestion? ChoiceQuestion { get; set; }

    public long OptionOrder { get; set; }

    public string? OptionValue { get; set; }

    public User? CreatedByUser { get; set; }

    public virtual User? UpdatedByUser { get; set; }

    public string? ErrorExplanation { get; set; }

    /*    public DateTime Field {  get; set; }*/

}