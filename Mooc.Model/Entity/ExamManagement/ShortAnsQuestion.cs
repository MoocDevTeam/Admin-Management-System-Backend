namespace Mooc.Model.Entity.ExamManagement;

public class ShortAnsQuestion : BaseQuestion
{
    public long? CreatedByUserId { get; set; }

    public long? UpdatedByUserId { get; set; }

    public long? CourseId { get; set; }

    public long QuestionTypeId { get; set; }

    public string? ReferenceAnswer { get; set; }

    //Navigation property
    public QuestionType? QuestionType { get; set; }

    public User? CreatedByUser { get; set; }

    public User? UpdatedByUser { get; set; }

    public CourseInstance? CourseInstance { get; set; }

    /*    public virtual ICollection<ExamQuestion>? ExamQuestions { get; set; }*/

    //public ICollection<Course>? CourseId { get; set; }
}

