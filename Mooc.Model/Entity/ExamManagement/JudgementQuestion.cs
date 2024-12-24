namespace Mooc.Model.Entity.ExamManagement;

public  class JudgementQuestion: BaseQuestion
{
    public bool CorrectAnswer {  get; set; }

    public QuestionType? QuestionType { get; set; }

    public User? CreatedByUser { get; set; }

    public ICollection<User>? UpdatedByUsers { get; set; }

    //public ICollection<Course>? CourseId { get; set; }
}