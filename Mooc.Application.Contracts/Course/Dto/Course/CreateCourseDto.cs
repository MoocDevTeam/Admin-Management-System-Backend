using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Mooc.Model.Entity;


namespace Mooc.Application.Contracts.Course
{
    public class CreateCourseDto : BaseEntityDto
    {
        [JsonIgnore] // hide Id
        public override long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public string Description { get; set; } = string.Empty;
        public long? CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }
        public long? CategoryId { get; set; }
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}