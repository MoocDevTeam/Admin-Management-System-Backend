namespace Mooc.Model.Entity.ExamManagement;

public class ChoiceQuestion : BaseQuestion
{
    public long? CreatedByUserId { get; set; }

    public long? UpdatedByUserId { get; set; }

    public long? CourseId { get; set; }

    public long QuestionTypeId { get; set; }

    public string? CorrectAnswer { get; set; }


    // Navigation property

    public virtual ICollection<Option>? Option { get; set; }

    public QuestionType? QuestionType { get; set; }

    public User? CreatedByUser { get; set; }

    public User? UpdatedByUser { get; set; }

    public CourseInstance? CourseInstance { get; set; }

/*    public virtual ICollection<ExamQuestion>? ExamQuestions { get; set; }*/
}