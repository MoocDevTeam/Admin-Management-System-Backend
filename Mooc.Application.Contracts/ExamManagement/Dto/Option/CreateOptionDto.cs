using System.Text.Json.Serialization;

namespace Mooc.Application.Contracts.ExamManagement;

public class CreateOptionDto : BaseEntityDto
{
    [JsonIgnore]
    public override long Id { get; set; }

    public long OptionOrder { get; set; }

    public string? OptionValue { get; set; }

    public string? ErrorExplanation { get; set; }

    [JsonIgnore]
    public long? CreatedByUserId { get; set; }

    [JsonIgnore]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public long? UpdatedByUserId { get; set; }

    [JsonIgnore]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}