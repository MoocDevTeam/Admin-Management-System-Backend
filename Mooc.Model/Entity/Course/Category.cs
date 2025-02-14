﻿using System.Text.Json.Serialization;

namespace Mooc.Model.Entity;

public class Category : BaseEntityWithAudit
{
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public string? IconUrl { get; set; }
    public long? ParentId { get; set; }

    // foreign keys
    //public long CreatedByUserId { get; set; }
    //public long? UpdatedByUserId { get; set; }

    // timestamp
    //public DateTime CreatedAt { get; set; }
    //public DateTime? UpdatedAt { get; set; }

    // Nav user
    public virtual User CreatedByUser { get; set; }
    public virtual User UpdatedByUser { get; set; }

    //Nav Category
    [JsonIgnore]
    public virtual Category ParentCategory { get; set; }
    public virtual ICollection<Category> ChildrenCategories { get; set; }

    // Navigation for Courses (one-to-many)
    [JsonIgnore]
    public ICollection<MoocCourse> Courses { get; set; } = new List<MoocCourse>();


}
