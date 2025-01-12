namespace Mooc.Application.Contracts.Course
{
    public class UpdateCourseInstanceDto : CreateOrUpdateCourseInstanceBaseDto
    {
        public long UpdatedByUserId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
