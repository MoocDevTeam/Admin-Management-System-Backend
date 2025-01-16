namespace Mooc.Model.Entity;
public class ExamPublish : BaseEntityWithAudit
{
  public long ExamId { get; set; }

/*  public DateTime PublishedAt { get; set; }

  public long? PublishedByUserId { get; set; }*/

  public DateTime? CloseAt { get; set; }

    /*  public int CourseInstanceId { get; set; }*/

    // Navigation propertye:

  public virtual Exam? Exam { get; set; }

  public virtual User? CreatedByUser { get; set; }

  public virtual User? UpdatedByUser { get; set; }

  //public User? PublishedByUser { get; set; }

 /*  public Course Course { get; set; }*/
}