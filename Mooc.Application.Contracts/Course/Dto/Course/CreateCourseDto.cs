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
        [Required(ErrorMessage = "Title is null")]
        [StringLength(100, ErrorMessage = "Title must be less than 100 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "CourseCode is null")]
        [StringLength(100, ErrorMessage = "CourseCode must be less than 100 characters")]
        public string CourseCode { get; set; }

        public string? CoverImage { get; set; }
        public string Description { get; set; } = "Description";
        [Required(ErrorMessage = "CategoryId is null")]
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