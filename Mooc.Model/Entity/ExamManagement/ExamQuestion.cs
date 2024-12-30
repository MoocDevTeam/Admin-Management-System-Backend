namespace Mooc.Model.Entity.ExamManagement;

public class ExamQuestion: BaseEntity
{
    public long examId { get; set; }
    public long questionId { get; set; }
    public int marks { get; set; }
    public int questionOrder { get; set; }
    public DateTime createdAt { get; set; }
    public long createdByUserId { get; set; }
    public long updatedByUserId { get; set; }
    public DateTime updatedAt { get; set; }
    public long questionTypeId { get; set; }
    
}