namespace Mooc.Model.Entity.ExamManagement;

public class ShortAnsQuestion : BaseQuestion
{
    public long QuestionTypeId { get; set; }

    public string? ReferenceAnswer { get; set; }

    //Navigation property
    public QuestionType? QuestionType { get; set; }

    public User? CreatedByUser { get; set; }

    public virtual ICollection<User>? UpdatedByUsers { get; set; }

/*    public virtual ICollection<ExamQuestion>? ExamQuestions { get; set; }*/

    //public ICollection<Course>? CourseId { get; set; }
}

