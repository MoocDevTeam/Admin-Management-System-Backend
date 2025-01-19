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
        [JsonIgnore]
        public new long Id { get; set; }
        public string Title { get; set; }
        public string CourseCode { get; set; }
        public string? CoverImage { get; set; }
        public string Description { get; set; } = "Description";
        public long CategoryId { get; set; } = 1;
        [JsonIgnore]
        public long? CreatedByUserId { get; set; } = 1;
        [JsonIgnore]
        public long? UpdatedByUserId { get; set; } = 1;
        [JsonIgnore]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}