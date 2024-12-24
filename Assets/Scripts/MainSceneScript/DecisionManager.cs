using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class DecisionManager : MonoBehaviour
{
    [Header("References")]
    public PassportManager passportManager;
    public TextMeshProUGUI resultText;
    public GameObject gameOverPanel;
    public UIManagerMainScene uiManager;

    [Header("Reputation Settings")]
    public Slider reputationBar;
    [SerializeField] private int maxReputation = 100;
    [SerializeField] private int reputationPenalty = 10;

    private int currentReputation;
    private bool isGameOver = false;

    private void Start()
    {
        currentReputation = maxReputation;

        if (uiManager == null)
        {
            Debug.LogError("UIManager is not assigned in the Inspector!");
        }

        uiManager.UpdateReputationBar(reputationBar, currentReputation, maxReputation);
        uiManager.UpdateResultText(resultText, "");
    }

    public void Approve()
    {
        if (isGameOver) return;
        EvaluateDecision(true);
    }

    public void Deny()
    {
        if (isGameOver) return;
        EvaluateDecision(false);
    }

    private void EvaluateDecision(bool isApproved)
    {
        Applicant applicant = passportManager.GetCurrentApplicant();
        if (applicant == null)
        {
            Debug.LogError("No applicant found for decision evaluation.");
            return;
        }

        Validator validator = new Validator();
        List<OriginSymbolPair> shuffledPairs = passportManager.GetShuffledPairs();
        if (shuffledPairs == null || shuffledPairs.Count == 0)
        {
            Debug.LogError("Shuffled pairs list is null or empty.");
            return;
        }

        bool isBirthYearValid = validator.IsValidBirthYear(applicant.BirthYear, 4065);
        bool isExpirationValid = validator.IsValidExpirationYear(applicant.ExpirationYear, 4065);
        bool isOriginValid = validator.IsValidOriginSymbol(applicant.Origin, applicant.RegionSymbol, shuffledPairs);

        if (isApproved)
        {
            if (isBirthYearValid && isExpirationValid && isOriginValid)
            {
                uiManager.UpdateResultText(resultText, "Correct Decision! Applicant Approved.");
            }
            else
            {
                // Trigger Game Over for a critical mistake
                GameOver("You approved an invalid applicant! Critical Error!");
                return; // Stop further execution
            }
        }
        else
        {
            if (!isBirthYearValid || !isExpirationValid || !isOriginValid)
            {
                uiManager.UpdateResultText(resultText, "Correct Decision! Unauthorized Applicant Denied.");
            }
            else
            {
                uiManager.UpdateResultText(resultText, "Wrong Decision! Valid applicant denied.");
                AdjustReputation(-reputationPenalty);
            }
        }

        if (!isGameOver) // Only proceed if the game is not over
        {
            Invoke(nameof(InvokeLoadNextApplicant), 1.5f);
        }
    }


    private void AdjustReputation(int amount)
    {
        currentReputation += amount;
        uiManager.UpdateReputationBar(reputationBar, currentReputation, maxReputation);

        if (currentReputation <= 0)
        {
            GameOver("Your reputation has dropped to zero! Game Over.");
        }
    }

    private void InvokeLoadNextApplicant()
    {
        passportManager.LoadNextApplicant(resultText, uiManager);
    }


    private void GameOver(string message)
    {
        isGameOver = true;

        // Disable all buttons
        uiManager.DisableAllButtons();

        // Show Game Over panel and update the message
        uiManager.HandleGameOver(gameOverPanel, resultText, message);

        Debug.Log(message);
    }

}
