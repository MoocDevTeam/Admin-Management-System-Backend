﻿namespace Mooc.Model.Entity;

public class Enrollment : BaseEntityWithAudit
{    
    public long CourseInstanceId { get; set; }

    public EnrollmentStatus EnrollmentStatus { get; set; }
    public DateTime EnrollStartDate { get; set; }
    public DateTime EnrollEndDate { get; set; }
    public int MaxStudents { get; set; }

    //public DateTime CreatedAt { get; set; }
    //public DateTime UpdatedAt { get; set; }

    //[ForeignKey("CreatedByUser")]
    //public long CreatedByUserId { get; set; }
    //[ForeignKey("UpdatedByUser")]
    //public long UpdatedByUserId { get; set; }

    //public virtual CourseInstance CourseInstance { get; set; }

    public virtual User CreatedByUser { get; set; }
    public virtual User UpdatedByUser { get; set; }
}
