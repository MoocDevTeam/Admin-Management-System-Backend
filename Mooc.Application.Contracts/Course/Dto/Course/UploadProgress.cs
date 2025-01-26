namespace Mooc.Application.Contracts.Course
{
    /// <summary>
    /// Represents the progress of an upload task.
    /// </summary>
    public class UploadProgress
    {
        /// <summary>
        /// The unique identifier for the upload task.
        /// </summary>
        public string UploadId { get; set; }

        /// <summary>
        /// The percentage of the upload that has been completed.
        /// </summary>
        public int Percentage { get; set; }
    }
}
