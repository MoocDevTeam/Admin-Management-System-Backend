using System;

namespace Mooc.Model.Entity;

public class Carousel : BaseEntityWithAudit
{
    /// <summary>
    /// The title of the carousel item.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The URL of the image displayed in the carousel.
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    /// The URL to redirect to when the carousel item is clicked.
    /// </summary>
    public string RedirectUrl { get; set; }

    /// <summary>
    /// Indicates whether the carousel item is active and visible.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The position of the carousel item in the display order.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// The start date for the carousel item's display period.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// The end date for the carousel item's display period.
    /// </summary>
    public DateTime EndDate { get; set; }
}
