public class UploadProgress
{
    public string UploadId { get; set; }
    public int Percentage { get; set; }

    public override string ToString()
    {
        return $"{Percentage}";
    }
}
