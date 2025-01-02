namespace Mooc.Model.Entity.ExamManagement;

public  class JudgementQuestion: BaseQuestion
{
    public long QuestionTypeId { get; set; }

    public bool CorrectAnswer {  get; set; }

    // Navigation property
    public QuestionType? QuestionType { get; set; }

    public User? CreatedByUser { get; set; }

    public virtual ICollection<User>? UpdatedByUsers { get; set; }

/*    public virtual ICollection<ExamQuestion>? ExamQuestions { get; set; }*/

    //public ICollection<Course>? CourseId { get; set; }
}