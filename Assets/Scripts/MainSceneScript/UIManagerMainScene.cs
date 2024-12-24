using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerMainScene : MonoBehaviour
{
    public void ShowPanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(true);
    }

    public void HidePanel(GameObject panel)
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void UpdateText(TextMeshProUGUI textElement, string message)
    {
        if (textElement != null)
            textElement.text = message;
    }

    public void DisableAllButtons()
    {
        Button[] allButtons = Object.FindObjectsByType<Button>(FindObjectsSortMode.None);
        foreach (Button button in allButtons)
        {
            button.interactable = false;
        }
    }
    public void UpdateResultText(TextMeshProUGUI resultText, string message)
    {
        if (resultText != null)
        {
            resultText.text = message;
        }
    }
    public void ShowGameOverPanel(GameObject panel, bool isGameOver, string message, TextMeshProUGUI textElement)
    {
        if (panel != null)
            panel.SetActive(true);

        if (textElement != null)
        {
            textElement.text = isGameOver
                ? "Game Over!\nYour reputation has dropped to zero!"
                : message;
        }
    }
    public void UpdateReputationBar(Slider reputationBar, int value, int maxValue)
    {
        if (reputationBar != null)
        {
            reputationBar.maxValue = maxValue;
            reputationBar.value = value;
        }
        else
        {
            Debug.LogError("ReputationBar is not assigned!");
        }
    }
    public void HandleGameOver(GameObject gameOverPanel, TextMeshProUGUI resultText, string message)
    {
        if (gameOverPanel != null)
        {
            ShowPanel(gameOverPanel); // Reuse ShowPanel method
        }

        if (resultText != null)
        {
            UpdateText(resultText, message); // Reuse UpdateText method
        }
    }




}
