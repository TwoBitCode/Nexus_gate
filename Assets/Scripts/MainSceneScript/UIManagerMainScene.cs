using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerMainScene : MonoBehaviour
{
    public Image applicantImage;
    public Image regionSymbol;
    public TextMeshProUGUI passportText;
    [Header("Coin UI")]
    public TextMeshProUGUI coinText; // Reference to the coin text UI


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
    public void UpdateApplicantUI(Applicant applicant)
    {
        if (applicant == null)
        {
            Debug.LogError("Applicant is null! Cannot update UI.");
            return;
        }

        // Ensure all necessary UI components are accessible
        if (applicantImage == null || regionSymbol == null || passportText == null)
        {
            Debug.LogError("One or more UI elements are not assigned in UIManagerMainScene.");
            return;
        }

        // Update UI elements with applicant data
        applicantImage.sprite = applicant.FaceImage;
        regionSymbol.sprite = applicant.RegionSymbol;
        passportText.text = $"Name: {applicant.Name}\n" +
                            $"Date of Birth: {applicant.BirthYear}\n" +
                            $"Origin: {applicant.Origin}\n" +
                            $"Expiration Year: {applicant.ExpirationYear}";

        Debug.Log("Applicant UI successfully updated.");
    }
    public void UpdateCoinsDisplay(int coins)
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {coins}";
        }
        else
        {
            Debug.LogError("Coin Text UI is not assigned in UIManagerMainScene!");
        }
    }






}
