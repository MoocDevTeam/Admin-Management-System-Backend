using Mooc.Shared.Enum;

namespace Mooc.Application.Contracts.Course
{
    public class TeacherCourseInstanceDto : BaseEntityDto
    {
        public TeacherCourseInstancePermissionType PermissionType { get; set; }
        public string Teacher { get; set; }
        public long CourseInstanceId { get; set; }
    }
}
