namespace Mooc.Model.Entity.ExamManagement;

public class ExamPublish: BaseEntity
{
    public long examId { get; set; }
    public DateTime publishedAt { get; set; }
    public long publishedByUserId { get; set; }
    public DateTime closedAt { get; set; }
    public long courseInstanceId { get; set; }
}