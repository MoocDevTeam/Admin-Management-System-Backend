namespace Mooc.Model.Entity;

public class ChoiceQuestion : BaseEntity
{
    /*    public long QuestionTypeId { get; set; }*/
    public long OptionId { get; set; }
    /*    public long CourseId { get; set; }*/
    public string CorrectAnswer { get; set; }
    public string QuestionBody { get; set; }
    public string QuestionTitle { get; set; }
    public int Marks { get; set; }
    /*    public QuestionType QuestionType { get; set; }*/
    public virtual ICollection<Option> Options { get; set; }
    /*    public Course Course { get; set; }*/
}