using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel;       // Reference to the GameOverPanel
    public TextMeshProUGUI gameOverText;   // Reference to the text in the GameOverPanel
    public TextMeshProUGUI coinSummaryText; // Reference for the earnings summary at the end of the day

    [Header("Managers")]
    public DayManager dayManager;         // Reference to the DayManager
    public UIManagerMainScene uiManager;  // Reference to the centralized UIManager

    [Header("Player Coins")]
    public int currentCoins = 0;          // Tracks player's total coins
    private int dailySalary = 100;        // Base salary for completing a day
    private int fineAmount = 50;          // Fine for invalid approvals
    private int finesIncurred = 0;        // Tracks total fines for the day

    public void AddFine()
    {
        finesIncurred += fineAmount;
        Debug.Log($"Fine added. Total fines: {finesIncurred}");
    }

    public void ResetDailyEarnings()
    {
        finesIncurred = 0; // Reset fines at the start of a new day
        Debug.Log("Daily earnings reset.");
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        Debug.Log($"Added {amount} coins. Total: {currentCoins}");
    }

    public void DeductCoins(int amount)
    {
        currentCoins -= amount;
        if (currentCoins < 0) currentCoins = 0;
        Debug.Log($"Deducted {amount} coins. Total: {currentCoins}");
    }

    public int CalculateEndOfDayEarnings(bool isSuccessfulDay)
    {
        if (!isSuccessfulDay) return 0; // If the game is over, no earnings for the day
        return dailySalary - finesIncurred;
    }

    public void ShowEndOfDayMessage(bool isGameOver)
    {
        if (uiManager == null)
        {
            Debug.LogError("UIManager is not assigned in GameController!");
            return;
        }

        // Calculate and update coins for the day
        int endOfDayEarnings = CalculateEndOfDayEarnings(!isGameOver);
        AddCoins(endOfDayEarnings);
        string fineDisplay = finesIncurred == 0 ? "0" : $"-{finesIncurred}";
        // Display earnings summary
        string coinSummary = isGameOver
    ? $"Game Over!\nYou earned {currentCoins} coins in total."
    : $"Day Complete!\nSalary: {dailySalary}\nFines: {fineDisplay}\nEarnings: {endOfDayEarnings}";
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
            ResetDailyEarnings(); // Reset fines for the new day
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
