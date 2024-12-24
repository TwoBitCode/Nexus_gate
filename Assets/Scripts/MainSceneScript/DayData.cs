using UnityEngine;

[CreateAssetMenu(fileName = "New DayData", menuName = "Game/DayData")]
public class DayData : ScriptableObject
{
    [Header("Daily Message Data")]
    public string[] dailyMessages;   // Messages for Daily Message Scene
    public AudioClip dailyMusic;     // Background music for the day

    [Header("Gameplay Rules")]
    public string[] newRules;        // Rules for the main game scene
    public OriginSymbolPair[] originSymbolPairs; // List of origins and their corresponding symbols

    [Header("Applicant Validation")]
    public bool checkDateOfBirth;    // Whether to check the date of birth
    public bool checkExpiration;     // Whether to check the expiration date
    public bool checkOriginSymbol;   // Whether to validate the origin matches the symbol
    public int maxApplicantsPerDay;  // Maximum number of applicants per day

    [Header("Applicant Data")]
    public string[] alienNames;      // Pool of alien names
    public Sprite[] alienImages;     // Pool of alien face images
    public string[] dialogueLines;   // Random dialogue lines for applicants
}
