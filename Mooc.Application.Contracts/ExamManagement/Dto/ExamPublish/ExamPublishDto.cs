namespace Mooc.Application.Contracts.ExamManagement;

public class ExamPublishDto : BaseEntityDto
{
    public long ExamId { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? UpdatedByUserId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CloseAt { get; set; }
}