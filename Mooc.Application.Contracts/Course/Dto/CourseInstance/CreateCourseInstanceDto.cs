namespace Mooc.Application.Contracts.Course
{
    public class CreateCourseInstanceDto : CreateOrUpdateCourseInstanceBaseDto
    {
        public long CreatedByUserId { get; set; }
        public long UpdatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
