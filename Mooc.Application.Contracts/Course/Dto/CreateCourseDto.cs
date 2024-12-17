using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Course
{
    public class CreateCourseDto : BaseEntityDto
    {
        public string Title { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public string Description { get; set; } = string.Empty;
        public long? CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}