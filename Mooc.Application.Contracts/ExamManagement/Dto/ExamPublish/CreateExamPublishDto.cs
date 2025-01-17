using System.Text.Json.Serialization;

namespace Mooc.Application.Contracts.ExamManagement;

public class CreateExamPublishDto : BaseEntityDto
{
    [JsonIgnore]
    public override long Id { get; set; }

    public long ExamId { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long? UpdatedByUserId { get; set; }

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CloseAt { get; set; } = DateTime.UtcNow;
}