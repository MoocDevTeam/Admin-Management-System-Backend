namespace Mooc.Model.Entity.ExamManagement;

public class BaseExam : BaseEntity
{
    public long? CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? UpdatedByUserId { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
