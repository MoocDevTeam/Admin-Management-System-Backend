using Mooc.Model.Entity.ExamManagement;

namespace Mooc.Model.Entity;
public class ExamQuestion : BaseExam
{
    public Exam Exam { get; set; }
    public long? ExamId { get; set; }

    public long? ChoiceQuestionId { get; set; }

    public long? JudgementQuestionId { get; set; }

    public long? ShortAnsQuestionId { get; set; }

    public ChoiceQuestion ChoiceQuestion { get; set; }

    public JudgementQuestion JudgementQuestion { get; set; }

    public ShortAnsQuestion ShortAnsQuestion { get; set; }

    public int Marks { get; set; }

    public int QuestionOrder { get; set; }






}