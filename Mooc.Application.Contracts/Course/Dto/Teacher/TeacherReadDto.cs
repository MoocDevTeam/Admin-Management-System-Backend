namespace Mooc.Application.Contracts.Course.Dto
{
    public class TeacherReadDto : BaseEntityDto
    {

        public string Title { get; set; }

        public string Department { get; set; }

        public string Introduction { get; set; }

        public string Expertise { get; set; }

        public string Office { get; set; }

        public DateTime HiredDate { get; set; }

        public bool IsActive { get; set; }

        public long CreatedByUserId { get; set; }

        public long UpdatedByUserId { get; set; }

        public string CreatedByUser { get; set; }

        public string UpdatedByUser { get; set; }

        public string DisplayName { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
