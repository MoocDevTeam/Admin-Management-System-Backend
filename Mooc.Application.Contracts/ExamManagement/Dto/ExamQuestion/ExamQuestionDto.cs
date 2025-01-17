namespace Mooc.Application.Contracts.ExamManagement;

public class ExamQuestionDto : BaseEntityDto
{
    public long CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? UpdatedByUserId { get; set; }

    public DateTime? UpdatedAt { get; set; }


    public long? ExamId { get; set; }

    public long? ChoiceQuestionId { get; set; }

    public long? JudgementQuestionId { get; set; }

    public long? ShortAnsQuestionId { get; set; }


    public int Marks { get; set; }

    public int QuestionOrder { get; set; }
}