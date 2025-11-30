using Google.Cloud.Firestore;

namespace shukuma.persistence.firebase;

[FirestoreData]
public class WorkoutSessionEntity
{
    [FirestoreProperty]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty]
    public string UserId { get; set; } = string.Empty;

    [FirestoreProperty]
    public string UserEmail { get; set; } = string.Empty;

    [FirestoreProperty]
    public int CardsCompleted { get; set; } = 0;

    [FirestoreProperty]
    public string TimeElapsed { get; set; } = string.Empty;

    [FirestoreProperty]
    public string StartedAt { get; set; } = string.Empty;

    [FirestoreProperty]
    public string CompletedAt { get; set; } = string.Empty;

    [FirestoreProperty]
    public bool IsCompleted { get; set; } = false;

    [FirestoreProperty]
    public int[] CardSequence { get; set; } = Array.Empty<int>();
}