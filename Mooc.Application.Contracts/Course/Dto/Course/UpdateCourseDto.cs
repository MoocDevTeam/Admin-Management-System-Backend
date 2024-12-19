using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mooc.Model.Entity;
using Mooc.Model.Entity.Course;

namespace Mooc.Application.Contracts.Course
{
    public class UpdateCourseDto : BaseEntityDto
    {
        public string Title { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public string Description { get; set; } = string.Empty;
        public User CreatedByUser { get; set; }
        public User UpdatedByUser { get; set; }
        public long CreatedByUserId { get; set; }
        public long UpdatedByUserId { get; set; }
        // public Category Category { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}