
using System;

namespace Mooc.Model.Entity;

public class Carousel : BaseEntity
{
    public string Title { get; set; }

    public string ImageUrl { get; set; }

    public string RedirectUrl { get; set; }

    public bool IsActive { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int Position { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public long CreatedByUserId { get; set; }

    public long UpdatedByUserId { get; set; }
}
