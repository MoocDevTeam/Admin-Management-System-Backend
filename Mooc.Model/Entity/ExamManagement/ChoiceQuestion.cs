namespace Mooc.Model.Entity.ExamManagement;

public class ChoiceQuestion : BaseQuestion
{
    public long QuestionTypeId { get; set; }

    public string? CorrectAnswer { get; set; }


    // Navigation property

    public virtual ICollection<Option>? Option { get; set; }

    public QuestionType? QuestionType { get; set; }

    public User? CreatedByUser { get; set; }

    public virtual ICollection<User>? UpdatedByUsers { get; set; }

    //public virtual ICollection<ExamQuestion>? ExamQuestions { get; set; }

    //public ICollection<Course>? CourseId { get; set; }
}