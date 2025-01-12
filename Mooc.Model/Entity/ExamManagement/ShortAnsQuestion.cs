namespace Mooc.Model.Entity.ExamManagement;

public class ShortAnsQuestion : BaseEntityWithAudit
{
    public long? CourseId { get; set; }

    public long QuestionTypeId { get; set; }

    public string? ReferenceAnswer { get; set; }

    public string? QuestionBody { get; set; }

    public string? QuestionTitle { get; set; }

    public int Marks { get; set; }

    //Navigation property
    public QuestionType? QuestionType { get; set; }

    public User? CreatedByUser { get; set; }

    public User? UpdatedByUser { get; set; }

    public CourseInstance? CourseInstance { get; set; }

    /*    public virtual ICollection<ExamQuestion>? ExamQuestions { get; set; }*/

    //public ICollection<Course>? CourseId { get; set; }
}

