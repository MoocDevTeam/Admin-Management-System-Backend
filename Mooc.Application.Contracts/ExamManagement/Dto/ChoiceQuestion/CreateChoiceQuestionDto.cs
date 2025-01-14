using System.Text.Json.Serialization;

namespace Mooc.Application.Contracts.ExamManagement;

public class CreateChoiceQuestionDto : BaseEntityDto
{
    [JsonIgnore]
    public override long Id { get; set; }

    public long CourseId { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedByUserId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? QuestionBody { get; set; }

    public string? QuestionTitle { get; set; }

    public int Marks { get; set; }



    public long QuestionTypeId { get; set; }

    public string? CorrectAnswer { get; set; }
}