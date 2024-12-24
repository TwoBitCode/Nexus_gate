using UnityEngine;

public class DayManager : MonoBehaviour
{
    public DayData[] allDayData; // Array of all DayData objects
    public int currentDay = 1;   // The current day in the game

    public DayData GetCurrentDayData()
    {
        if (allDayData != null && currentDay - 1 < allDayData.Length)
        {
            return allDayData[currentDay - 1];
        }

        Debug.LogError("No DayData available for the current day!");
        return null;
    }

    public bool HasNextDay()
    {
        return currentDay < allDayData.Length;
    }

    public void AdvanceToNextDay()
    {
        if (HasNextDay())
        {
            currentDay++;
        }
        else
        {
            Debug.LogWarning("No more days left to advance to.");
        }
    }
}
