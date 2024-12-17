using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI timeText;  // Reference to UI Text to display time

    [Header("Time Settings")]
    public int currentYear = 4065;    // Starting game year
    public int currentDay = 1;        // Day in the game
    public int startHour = 7;         // Starting hour (e.g., 7 AM)
    public int startMinute = 0;       // Starting minute
    public float minuteDuration = 1f; // Duration of each in-game minute in seconds

    private float timeCounter;        // Tracks elapsed time
    private int currentHour;          // Current hour
    private int currentMinute;        // Current minute

    private void Start()
    {
        // Initialize the day, hour, and minute
        currentHour = startHour;
        currentMinute = startMinute;
        timeCounter = 0f;
        UpdateTimeDisplay();
    }

    private void Update()
    {
        // Increment time
        timeCounter += Time.deltaTime;

        if (timeCounter >= minuteDuration)
        {
            timeCounter = 0f;   // Reset time counter
            IncrementTime();    // Update game time
        }
    }

    private void IncrementTime()
    {
        currentMinute++; // Add one minute

        if (currentMinute >= 60) // If we reach the next hour
        {
            currentMinute = 0;  // Reset minutes
            currentHour++;      // Increment hour

            if (currentHour >= 24) // If day ends
            {
                currentHour = 0; // Reset hour to midnight
                currentDay++;    // Advance to the next day
            }
        }

        UpdateTimeDisplay();
    }

    private void UpdateTimeDisplay()
    {
        // Format and display the time
        string timeString = $"Year: {currentYear} | Day: {currentDay} | Time: {currentHour:00}:{currentMinute:00}";
        timeText.text = timeString;
    }
}
