using System.Text.Json.Serialization;

namespace Mooc.Application.Contracts.ExamManagement;

public class CreateOptionDto : BaseEntityDto
{
    [JsonIgnore]
    public override long Id { get; set; }

    public long ChoiceQuestionId { get; set; }

    public long OptionOrder { get; set; }

    public string? OptionValue { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedByUserId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? ErrorExplanation { get; set; }
}