using Google.Cloud.Firestore;

namespace shukuma.persistence.firebase;

[FirestoreData]
public class UserEntity
{
    [FirestoreProperty]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty]
    public string FirstName { get; set; } = string.Empty;

    [FirestoreProperty]
    public string LastName { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Username { get; set; } = string.Empty; // ADDED: Missing field

    [FirestoreProperty]
    public string ContactNumber { get; set; } = string.Empty;

    [FirestoreProperty]
    public string EmailAddress { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Password { get; set; } = string.Empty;

    [FirestoreProperty]
    public string CreatedBy { get; set; } = string.Empty;

    [FirestoreProperty]
    public bool HasCompletedChallenge { get; set; } = false;

    [FirestoreProperty]
    public string CompletedBy { get; set; } = string.Empty;

    [FirestoreProperty]
    public string TimeCompleted { get; set; } = string.Empty;

    [FirestoreProperty]
    public int LastCardNumber { get; set; } = default;

    [FirestoreProperty]
    public string CardsCompleted { get; set; } = string.Empty;

    [FirestoreProperty]
    public string? Review { get; set; }
}