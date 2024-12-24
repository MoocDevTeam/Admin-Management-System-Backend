namespace Mooc.Model.Entity.ExamManagement;

public class Option : BaseEntity
{
    public long ChoiceQuestionId { get; set; }

    public ChoiceQuestion? ChoiceQuestion { get; set; }

    public long OptionOrder { get; set; }

    public string? OptionValue { get; set; }

    public long CreatedByUserId { get; set; }

    public User? CreatedByUser { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedByUserId { get; set; }

    public ICollection<User>? UpdatedByUsers { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? ErrorExplanation { get; set; }

    public DateTime Field {  get; set; }

}