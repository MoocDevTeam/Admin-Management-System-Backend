namespace Mooc.Application.Contracts.ExamManagement;

public class ShortAnsQuestionDto : BaseEntityDto
{
    public long CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public long UpdatedByUserId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? QuestionBody { get; set; }

    public string? QuestionTitle { get; set; }

    public int Marks { get; set; }



    public long QuestionTypeId { get; set; }

    public string? ReferenceAnswer { get; set; }
}