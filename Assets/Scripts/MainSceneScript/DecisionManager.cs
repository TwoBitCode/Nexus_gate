using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class DecisionManager : MonoBehaviour
{
    [Header("References")]
    public ApplicantManager applicantManager;
    public TextMeshProUGUI resultText;
    public GameObject gameOverPanel;
    public UIManagerMainScene uiManager;

    [Header("Reputation Settings")]
    public Slider reputationBar;
    [SerializeField] private int maxReputation = 100;
    [SerializeField] private int reputationPenalty = 10;

    private int currentReputation;
    private bool isGameOver = false;
    public UIApplicantPanelManager uiApplicantPanelManager; // Reference to UIApplicantPanelManager
    [Header("Game Controller")]
    public GameController gameController;

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
        Applicant applicant = applicantManager.GetCurrentApplicant(); // Use ApplicantManager

        if (applicant == null)
        {
            Debug.LogError("No applicant found for decision evaluation.");
            return;
        }

        Validator validator = new Validator();
        List<OriginSymbolPair> shuffledPairs = applicantManager.GetShuffledPairs(); // If required

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
                AdjustReputation(-10); // Penalty for approving an invalid applicant
                GameOver("You approved an invalid applicant! Critical Error.");
                return; // Exit early
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
                AdjustReputation(-10);
                uiManager.UpdateResultText(resultText, "Wrong Decision! Valid applicant denied.");

            }
        }

        if (!isGameOver) // Only proceed if the game is not over
        {
            Invoke(nameof(InvokeLoadNextApplicant), 1.5f);
        }
    }

    private void AdjustReputation(int amount)
    {
        Debug.Log($"AdjustReputation called. Current Reputation: {currentReputation}, Adjustment: {amount}");

        // Adjust reputation
        currentReputation += amount;

        // Ensure reputation stays within bounds
        currentReputation = Mathf.Clamp(currentReputation, 0, maxReputation);

        // Update the UI
        uiManager.UpdateReputationBar(reputationBar, currentReputation, maxReputation);

        Debug.Log($"Reputation after adjustment: {currentReputation}");

        // Check if reputation has dropped to zero
        if (currentReputation <= 0)
        {
            GameOver("Your reputation has dropped to zero! Game Over.");
        }
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


    private void InvokeLoadNextApplicant()
    {
        // Check if all applicants have been processed
        if (applicantManager.AreAllApplicantsProcessed())
        {
            if (currentReputation > 0)
            {
                // Day Complete
                gameController.ShowEndOfDayMessage(false);
            }
            else
            {
                // Game Over
                gameController.ShowEndOfDayMessage(true);
            }
            return;
        }

        // Generate the next applicant
        if (applicantManager.GenerateNextApplicant())
        {
            Applicant nextApplicant = applicantManager.GetCurrentApplicant();
            if (nextApplicant != null)
            {
                uiApplicantPanelManager.UpdateApplicantUI(nextApplicant); // Correct UI update call
                uiManager.UpdateResultText(resultText, ""); // Reset result text for the next applicant
            }

            else
            {
                Debug.LogError("Failed to retrieve the next applicant!");
            }
        }
        else
        {
            Debug.Log("No more applicants to load.");
        }
    }



}
