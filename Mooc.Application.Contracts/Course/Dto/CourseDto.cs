using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Course
{
    public class CourseDto : BaseEntityDto
    {
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(50)]
        public string CourseCode { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;
        public long? CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}