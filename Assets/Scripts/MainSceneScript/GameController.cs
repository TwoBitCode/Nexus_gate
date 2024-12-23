using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel;       // Reference to the GameOverPanel
    public TextMeshProUGUI gameOverText;   // Reference to the text in the GameOverPanel

    [Header("Managers")]
    public DayManager dayManager;         // Reference to the DayManager
    public UIManagerMainScene uiManager;  // Reference to the centralized UIManager
    public EconomyManager economyManager; // Reference to the EconomyManager

    private void Start()
    {
        // Initialize EconomyManager and ensure GameManager is ready
        if (economyManager == null)
        {
            Debug.LogError("EconomyManager is not assigned in GameController!");
        }

        // Ensure GameManager is initialized
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager is not active in the scene!");
        }
    }

    public void ShowEndOfDayMessage(bool isGameOver)
    {
        if (uiManager == null)
        {
            Debug.LogError("UIManager is not assigned in GameController!");
            return;
        }

        // Calculate earnings for the day
        int endOfDayEarnings = economyManager.CalculateEndOfDayEarnings(!isGameOver);

        // Update GameManager's coins
        GameManager.Instance.AddCoins(endOfDayEarnings);

        // Format the fines and earnings for display
        string fineDisplay = economyManager.GetFines() == 0 ? "0" : $"-{economyManager.GetFines()}";
        string summaryMessage = isGameOver
            ? $"Game Over!\nTotal Coins Earned: {GameManager.Instance.coins}\nFinal Reputation: {GameManager.Instance.reputation}"
            : $"Day Complete!\nSalary: {economyManager.GetDailySalary()}\nFines: {fineDisplay}\nEarnings: {endOfDayEarnings}\nReputation: {GameManager.Instance.reputation}";

        // Update the game over text and show the panel
        uiManager.UpdateText(gameOverText, summaryMessage);
        uiManager.ShowPanel(gameOverPanel);
        uiManager.DisableAllButtons();

        // Log the status
        Debug.Log(isGameOver ? "Game Over: Ending game session." : "Day Complete: Proceed to the next day.");
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
            // Reset fines and proceed to the next day
            economyManager.ResetDailyEarnings();

            // Ensure EconomyManager uses the current coins from GameManager
            economyManager.AddCoins(GameManager.Instance.coins);

            // Advance to the next day
            dayManager.AdvanceToNextDay();
            dayManager.InitializeDay();
            Debug.Log($"Proceeding to Day {dayManager.currentDay}");
        }
        else
        {
            // If no more days, end the game
            ShowEndOfDayMessage(true);
        }
    }
}
