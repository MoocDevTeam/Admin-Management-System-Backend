using Mooc.Shared.Enum;
namespace Mooc.Application.Contracts.Course
{
    public class TeacherCourseInstanceReadDto : BaseEntityDto
    {
        public TeacherCourseInstancePermissionType PermissionType { get; set; }
        public string CreatedByUser { get; set; }
        public string UpdatedByUser { get; set; }

    }
}
