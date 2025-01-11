using Mooc.Shared.Enum;

namespace Mooc.Application.Contracts.Course
{
    public class TeacherCourseInstanceCreateOrUpdateDto : BaseEntityDto
    {
        public TeacherCourseInstancePermissionType PermissionType { get; set; }
        public long TeacherId { get; set; }
        public long CourseInstanceId { get; set; }
    }
}
