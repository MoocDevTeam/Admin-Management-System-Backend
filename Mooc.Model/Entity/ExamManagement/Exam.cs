using Mooc.Model.Entity.ExamManagement;

namespace Mooc.Model.Entity;
public class Exam : BaseEntityWithAudit
{
    //public long CourseInstanceId { get; set; }

    public string? ExamTitle { get; set; }

    public int? Remark { get; set; }

    public int ExaminationTime { get; set; }

    public QuestionUpload AutoOrManual { get; set; }

    public int TotalScore { get; set; }

    public int TimePeriod { get; set; }

    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }

    public ExamPublish? ExamPublish { get; set; }
}