using Mooc.Shared.Enum;
namespace Mooc.Application.Contracts.Course
{
    public class TeacherCourseInstancePermissionDto : BaseEntityDto
    {
        public TeacherCourseInstancePermissionType PermissionType { get; set; }

    }
}
