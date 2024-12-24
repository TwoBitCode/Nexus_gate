using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel;       // Reference to the GameOverPanel
    public TextMeshProUGUI gameOverText;   // Reference to the text in the GameOverPanel

    [Header("Managers")]
    public DayManager dayManager;         // Reference to the DayManager
    public UIManagerMainScene uiManager;  // Reference to the centralized UIManager
    public EconomyManager economyManager; // Reference to the EconomyManager

    public void ShowEndOfDayMessage(bool isGameOver)
    {
        if (uiManager == null)
        {
            Debug.LogError("UIManager is not assigned in GameController!");
            return;
        }

        // Calculate earnings through EconomyManager
        int endOfDayEarnings = economyManager.CalculateEndOfDayEarnings(!isGameOver);

        // Add coins for the day to the total in EconomyManager
        economyManager.AddCoins(endOfDayEarnings);

        // Format fines display
        string fineDisplay = economyManager.GetFines() == 0 ? "0" : $"-{economyManager.GetFines()}";

        // Create the summary message
        string coinSummary = isGameOver
            ? $"Game Over!\nYou earned {economyManager.GetTotalCoins()} coins in total."
            : $"Day Complete!\nSalary: {economyManager.GetDailySalary()}\nFines: {fineDisplay}\nEarnings: {endOfDayEarnings}";

        // Update UI
        uiManager.UpdateText(gameOverText, coinSummary);
        uiManager.ShowPanel(gameOverPanel);
        uiManager.DisableAllButtons();

        if (isGameOver)
        {
            Debug.Log("Game Over: Ending game session.");
        }
        else
        {
            Debug.Log("Day Complete: Proceed to the next day.");
        }
    }

    public void ProceedToNextDay()
    {
        if (dayManager == null)
        {
            Debug.LogError("DayManager is not assigned in GameController!");
            return;
        }

        if (dayManager.HasNextDay())
        {
            economyManager.ResetDailyEarnings(); // Reset fines for the new day
            dayManager.AdvanceToNextDay();
            dayManager.InitializeDay();
            Debug.Log($"Proceeding to Day {dayManager.currentDay}");
        }
        else
        {
            ShowEndOfDayMessage(true);
        }
    }
}
