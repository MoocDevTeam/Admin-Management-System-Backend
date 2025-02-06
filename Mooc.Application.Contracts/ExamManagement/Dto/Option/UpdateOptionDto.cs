namespace Mooc.Application.Contracts.ExamManagement;
using System.Text.Json.Serialization;

public class UpdateOptionDto : BaseEntityDto
{
    public long ChoiceQuestionId { get; set; }

    public long OptionOrder { get; set; }

    public string? OptionValue { get; set; }

    [JsonIgnore]
    public long? CreatedByUserId { get; set; }

    [JsonIgnore]
    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public long? UpdatedByUserId { get; set; }

    [JsonIgnore]
    public DateTime? UpdatedAt { get; set; }

    public string? ErrorExplanation { get; set; }
}