﻿using Mooc.Shared.Enum;
namespace Mooc.Model.Entity
{
    public class TeacherCourseInstance : BaseEntityWithAudit
    {
        public TeacherCourseInstancePermissionType PermissionType { get; set; }
        public long TeacherId { get; set;}
        
        //Change this to CourseInstanceId when Kwon updates his part
        public long CourseInstanceId { get; set; } 

        //Nav attributes

        public User CreatedByUser { get; set; }
        public User UpdatedByUser { get; set; }
        public Teacher Teacher { get; set; }
        // need to modify this to { CourseInstance }when courseInstance updated
        public CourseInstance CourseInstance { get; set; }
    }
}
