using Mooc.Core.CustomValidation;

namespace Mooc.Application.Contracts.Course.Dto
{
    public class CreateOrUpdateCourseInstanceBaseDto : BaseEntityDto
    {
        [Required(ErrorMessage = "MoocCourseId is required.")]
        public long MoocCourseId { get; set; }

        [Required(ErrorMessage = "TotalSessions is required.")]
        [Range(1, 100, ErrorMessage = "TotalSessions must be a valid number between 1 to 100.")]
        public int TotalSessions { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public CourseInstanceStatus Status { get; set; }

        [Required(ErrorMessage = "Permission is required.")]
        public CourseInstancePermission Permission { get; set; }

        // reusable custom validation attribute
        [DateTimeValidation("StartDate", "EndDate")]
        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; set; }
    }
}