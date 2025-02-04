using System.Text.Json.Serialization;

namespace Mooc.Application.Contracts.ExamManagement;

public class CreateExamQuestionDto : BaseEntityDto
{
    [JsonIgnore]
    public override long Id { get; set; }

    [JsonIgnore]
    public long CreatedByUserId { get; set; }

    [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public long? UpdatedByUserId { get; set; }

    [JsonIgnore]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public long? ExamId { get; set; }

    public long? ChoiceQuestionId { get; set; }

    public long? JudgementQuestionId { get; set; }
    
    public long? ShortAnsQuestionId { get; set; }

    public int Marks { get; set; }

    public int QuestionOrder { get; set; }
}