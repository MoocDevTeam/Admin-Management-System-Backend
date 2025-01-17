namespace Mooc.Model.Entity;

public class ChoiceQuestion : BaseEntityWithAudit
{
    public string? QuestionBody { get; set; }

    public string? QuestionTitle { get; set; }

    public int Marks { get; set; }

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