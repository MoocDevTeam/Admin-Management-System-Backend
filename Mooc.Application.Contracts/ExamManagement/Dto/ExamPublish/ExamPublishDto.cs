namespace Mooc.Application.Contracts.ExamManagement;

public class ExamPublishDto : BaseEntityDto
{
    public long ExamId { get; set; }

    public DateTime PublishedAt { get; set; }

    public long PublishedByUserId { get; set; }

    public DateTime? CloseAt { get; set; }
}