namespace Mooc.Application.Contracts.Course.Dto
{
    public class CreateOrUpdateTeacherDto : BaseEntityDto
    {
        [Required(ErrorMessage = "Title is null")]
        [StringLength(100, ErrorMessage = "Title must be less than 100 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Department is null")]
        [StringLength(100, ErrorMessage = "Department must be less than 100 characters")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Introduction is null")]
        [StringLength(500, ErrorMessage = "Introduction must be less than 500 characters")]
        public string Introduction { get; set; }

        [Required(ErrorMessage = "Expertise is null")]
        [StringLength(100, ErrorMessage = "Expertise must be less than 100 characters")]
        public string Expertise { get; set; }

        [Required(ErrorMessage = "Office is null")]
        [StringLength(50, ErrorMessage = "Office must be less than 100 characters")]
        public string Office { get; set; }

        [Required(ErrorMessage = "Hired date is null")]
        public DateTime HiredDate { get; set; }

        [Required(ErrorMessage = "There must be a user to be assigned to a teacher role.")]
        public long UserId { get; set; }

        //To show if this teacher is still hired or has course assigned
        public bool IsActive { get; set; }
        public object CourseInstanceId { get; set; }
    }
}
