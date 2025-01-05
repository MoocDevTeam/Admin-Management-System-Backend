using Mooc.Model.Entity.ExamManagement;

namespace Mooc.Model.Entity;
public class ExamPublish : BaseExam
{
  public long ExamId { get; set; }

/*  public DateTime PublishedAt { get; set; }

  public long? PublishedByUserId { get; set; }*/

  public DateTime? CloseAt { get; set; }

    /*  public int CourseInstanceId { get; set; }*/

    // Navigation propertye:

  public Exam? Exam { get; set; }

  public User? CreatedByUser { get; set; }

  public User? UpdatedByUser { get; set; }

  //public User? PublishedByUser { get; set; }

 /*  public Course Course { get; set; }*/
}