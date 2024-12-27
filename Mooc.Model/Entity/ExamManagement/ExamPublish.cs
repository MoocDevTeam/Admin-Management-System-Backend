namespace Mooc.Model.Entity;
public class ExamPublish:BaseEntity
{
  public long ExamId { get; set; }

  public DateTime PublishedAt { get; set; }

  public long PublishedByUserId { get; set; }

  public DateTime? CloseAt { get; set; }

  /*  public int CourseInstanceId { get; set; }*/

  // foreign key reference:

  public Exam? Exam { get; set; }

  public User? PublishedByUser { get; set; }

 /*  public Course Course { get; set; }*/
}