namespace Mooc.Application.Contracts.ExamManagement;

public class UpdateExamPublishDto : BaseEntityDto
{
    public long ExamId { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long? UpdatedByUserId { get; set; }

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CloseAt { get; set; } = DateTime.UtcNow;
}