using System.Text.Json.Serialization;

namespace Mooc.Application.Contracts.ExamManagement;

public class CreateExamDto : BaseEntityDto
{
    [JsonIgnore]
    public override long Id { get; set; }

    public long CourseId { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public long? UpdatedByUserId { get; set; }

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    public string ExamTitle { get; set; }

    public int? Remark { get; set; }

    public int ExaminationTime { get; set; }

    public QuestionUpload AutoOrManual { get; set; }

    public int TotalScore { get; set; }

    public int TimePeriod { get; set; }
}