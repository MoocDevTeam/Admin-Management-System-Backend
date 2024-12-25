namespace Mooc.Model.Entity.ExamManagement;
public class Exam: BaseEntity   
{
    public long courseId { get; set; }
    public string examTitle { get; set; }
    public int remark { get; set; }
    public int examinationTime { get; set; }
    public DateTime createdAt { get; set; }
    public long createdByUserId { get; set; }
    public DateTime updatedAt { get; set; }
    public long updatedByUserId { get; set; }
    public AutoOrMannual autoOrMannual { get; set; }
    public int totalScore { get; set; }
    public int timePeriod { get; set; }
    
}