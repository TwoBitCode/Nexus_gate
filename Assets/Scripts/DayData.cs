using UnityEngine;

[CreateAssetMenu(fileName = "New DayData", menuName = "Game/DayData")]
public class DayData : ScriptableObject
{
    [Header("Daily Message Data")]
    public string[] dailyMessages;   // Messages for Daily Message Scene
    public AudioClip dailyMusic;     // Background music for the day

    [Header("Gameplay Rules")]
    public string[] newRules;        // Rules for the main game scene

    [Header("Characters and Events")]
    public GameObject[] charactersForTheDay; // Prefabs or GameObjects for the main game
    public string[] importantEvents;         // Special instructions/events
}
