using System.Text.Json.Serialization;

namespace Mooc.Application.Contracts.ExamManagement;

public class CreateQuestionTypeDto : BaseEntityDto
{
    [JsonIgnore]
    public override long Id { get; set; }

    public string QuestionTypeName { get; set; }
}