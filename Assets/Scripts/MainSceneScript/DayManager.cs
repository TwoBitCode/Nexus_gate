using UnityEngine;

public class DayManager : MonoBehaviour
{
    public DayData[] allDayData;
    public int currentDay = 1;
    public AudioSource audioSource;

    public void InitializeDay()
    {
        DayData dayData = GetCurrentDayData();
        if (dayData == null) return;

        // Play music
        PlayDailyMusic(dayData);
        Debug.Log($"Initializing Day {currentDay}");
    }

    public DayData GetCurrentDayData()
    {
        if (allDayData != null && currentDay - 1 < allDayData.Length)
        {
            return allDayData[currentDay - 1];
        }

        Debug.LogError("No DayData available for the current day!");
        return null;
    }

    public void PlayDailyMusic(DayData dayData)
    {
        if (audioSource == null || dayData.dailyMusic == null)
        {
            Debug.LogError("AudioSource or dailyMusic is not assigned!");
            return;
        }

        audioSource.clip = dayData.dailyMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopDailyMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public bool HasNextDay()
    {
        return currentDay < allDayData.Length;
    }

    public void AdvanceToNextDay()
    {
        if (HasNextDay())
        {
            StopDailyMusic();
            currentDay++;
        }
        else
        {
            Debug.LogWarning("No more days left to advance to.");
        }
    }
}
