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

        string message = isGameOver
            ? "Game Over!\nYou have lost all your reputation."
            : "Day Complete!\nGet ready for the next level.";

        uiManager.UpdateText(gameOverText, message);
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
