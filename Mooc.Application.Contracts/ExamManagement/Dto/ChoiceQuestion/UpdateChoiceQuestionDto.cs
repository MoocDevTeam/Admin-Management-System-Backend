namespace Mooc.Application.Contracts.ExamManagement;

public class UpdateChoiceQuestionDto : BaseEntityDto
{
    public long? CourseId { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public long UpdatedByUserId { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string? QuestionBody { get; set; }

    public string? QuestionTitle { get; set; }

    public int Marks { get; set; }

    public long QuestionTypeId { get; set; }

    public string? CorrectAnswer { get; set; }
}