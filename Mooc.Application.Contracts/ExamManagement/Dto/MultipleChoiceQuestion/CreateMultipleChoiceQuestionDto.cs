using System.Text.Json.Serialization;

namespace Mooc.Application.Contracts.ExamManagement;

public class CreateMultipleChoiceQuestionDto : BaseEntityDto
{
    [JsonIgnore]
    public override long Id { get; set; }
    
    public long? CourseId { get; set; }
    
    [JsonIgnore]
    public long? CreatedByUserId { get; set; }
    
    [JsonIgnore]
    public DateTime? CreatedAt { get; set; }
    
    [JsonIgnore]
    public long? UpdatedByUserId { get; set; }
    
    [JsonIgnore]
    public DateTime? UpdatedAt { get; set; }
    
    public string? QuestionBody { get; set; }
    
    public string? QuestionTitle { get; set; }
    
    public int Marks { get; set; }
    
    public long QuestionTypeId { get; set; }
    
    public string? CorrectAnswers { get; set; }
    
    public List<CreateOptionDto>? Options { get; set; }
}
