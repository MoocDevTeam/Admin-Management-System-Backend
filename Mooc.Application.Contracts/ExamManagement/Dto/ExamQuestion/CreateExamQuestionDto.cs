namespace Mooc.Application.Contracts.ExamManagemen;

public class CreateExamQuestionDto : BaseEntityDto
{
    public long CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? UpdatedByUserId { get; set; }

    public DateTime? UpdatedAt { get; set; }


    public long? ExamId { get; set; }

    public long? ChoiceQuestionId { get; set; }

    public long? JudgementQuestionId { get; set; }

    public long? ShortAnsQuestionId { get; set; }

    //public long QuestionId { get; set; }   // the foreign key will be removed if choose 1 column(questionId) and we will use a fake foreign key  Discriminator: 

    //public string? QuestionType { get; set; } // Discriminator: "Choice", "Judgement", or "ShortAnswer"

    public int Marks { get; set; }

    public int QuestionOrder { get; set; }

    public int QuestionTypeId { get; set; }
}
