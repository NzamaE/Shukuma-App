namespace shukuma.domain.Models;

public class FirestoreAppOptions
{
    public string ProjectId { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string BucketUrl { get; set; } = string.Empty;
    public Bucket Bucket { get; set; } = new();
    public Collection Collection { get; set; } = new();
}

public class Bucket
{
    public string Url { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
}

public class Collection
{
    public string User { get; set; } = string.Empty;
    public string WorkoutSession { get; set; } = string.Empty;
}