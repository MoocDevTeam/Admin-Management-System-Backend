namespace Mooc.Model.Entity.ExamManagement;

public class BaseQuestion: BaseEntity
{
    public long QuestionTypeId { get; set; }

    public long CourseId { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedByUserId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? QuestionBody {  get; set; }

    public string? QuestionTitle {  get; set; }

    public int Marks { get; set; }
}

