using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel;      // Reference to the GameOverPanel
    public TextMeshProUGUI gameOverText;  // Reference to the text in the GameOverPanel

    public void ShowEndOfDayMessage(bool isGameOver)
    {
        if (gameOverPanel == null || gameOverText == null)
        {
            Debug.LogError("GameOverPanel or GameOverText is not assigned in the GameController!");
            return;
        }

        // Set the appropriate message
        if (isGameOver)
        {
            gameOverText.text = "Game Over!\nYou have lost all your reputation.";
        }
        else
        {
            gameOverText.text = "Day Complete!\nGet ready for the next level.";
        }

        // Disable all buttons
        DisableAllButtons();

        // Show the GameOverPanel
        gameOverPanel.SetActive(true);
    }

    private void DisableAllButtons()
    {
        Button[] allButtons = Object.FindObjectsByType<Button>(FindObjectsSortMode.None); // Updated to use FindObjectsByType
        foreach (Button button in allButtons)
        {
            button.interactable = false; // Disable each button
        }

        Debug.Log("All buttons have been disabled.");
    }
}

