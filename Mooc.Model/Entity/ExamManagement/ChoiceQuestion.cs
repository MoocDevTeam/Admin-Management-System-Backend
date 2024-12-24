namespace Mooc.Model.Entity.ExamManagement;

public class ChoiceQuestion : BaseQuestion
{
    public string? CorrectAnswer { get; set; }

    public ICollection<Option>? Option { get; set; }

    public QuestionType? QuestionType { get; set; }

    public User? CreatedByUser { get; set; }

    public ICollection<User>? UpdatedByUsers { get; set; }

    //public ICollection<Course>? CourseId { get; set; }
}