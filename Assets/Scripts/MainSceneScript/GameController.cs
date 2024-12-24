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

    public void ShowEndOfDayMessage(bool isGameOver)
    {
        if (uiManager == null)
        {
            Debug.LogError("UIManager is not assigned in GameController!");
            return;
        }

        // Set the appropriate message
        string message = isGameOver
            ? "Game Over!\nYou have lost all your reputation."
            : "Day Complete!\nGet ready for the next level.";

        // Use UIManager to update the UI
        uiManager.UpdateText(gameOverText, message);
        uiManager.ShowPanel(gameOverPanel);

        // Disable all buttons
        uiManager.DisableAllButtons();

        if (!isGameOver)
        {
            Debug.Log("Day Complete: Prepare for the next day.");
        }
        else
        {
            Debug.Log("Game Over: Ending game session.");
        }
    }

    public void ProceedToNextDay()
    {
        if (dayManager == null)
        {
            Debug.LogError("DayManager is not assigned in GameController!");
            return;
        }

        // Check if there are more days
        if (dayManager.HasNextDay())
        {
            dayManager.AdvanceToNextDay();
            Debug.Log($"Proceeding to Day {dayManager.currentDay}");
        }
        else
        {
            ShowEndOfDayMessage(true); // End the game if no more days
        }
    }
}
