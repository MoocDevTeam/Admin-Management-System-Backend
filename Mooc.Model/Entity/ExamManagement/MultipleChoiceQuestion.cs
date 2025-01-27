public class MultipleChoiceQuestion : BaseEntityWithAudit
{
    public string? QuestionBody { get; set; }
    
    public string? QuestionTitle { get; set; }
    
    public int Marks { get; set; }
    
    public long? CourseId { get; set; }
    
    public long QuestionTypeId { get; set; }
    
    public string? CorrectAnswers { get; set; }  // Store multiple correct answers, e.g. "A,B,C"
    
    // Navigation properties
    public virtual ICollection<Option>? Options { get; set; }
    
    public QuestionType? QuestionType { get; set; }
    
    public User? CreatedByUser { get; set; }
    
    public User? UpdatedByUser { get; set; }
    
    public CourseInstance? CourseInstance { get; set; }
}
