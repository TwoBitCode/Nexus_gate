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
    //private bool isPassportOpened = false; // Tracks if the passport was opened


    public UIApplicantPanelManager uiApplicantPanelManager;
    [Header("Game Controller")]
    public GameController gameController;

    // Constants
    private const int CurrentYear = 4065; // Game's current year
    private const float DecisionDelay = 1.5f; // Delay before loading the next applicant

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
        if (!uiApplicantPanelManager.IsPassportOpened)
        {
            Debug.LogError("Cannot approve: Passport not opened.");
            uiManager.UpdateResultText(resultText, "You must open the passport before making a decision!");
            return;
        }

        // Proceed with evaluation
        EvaluateDecision(true);
    }

    public void Deny()
    {
        if (!uiApplicantPanelManager.IsPassportOpened)
        {
            // Show a friendly message in the result text area
            uiManager.UpdateResultText(resultText, "Please open the passport before making a decision.");
            Debug.Log("Player attempted to deny without opening the passport.");
            return;
        }

        // Proceed with evaluation
        EvaluateDecision(false);
    }

    private void EvaluateDecision(bool isApproved)
    {
        Applicant applicant = applicantManager.GetCurrentApplicant();

        if (applicant == null)
        {
            Debug.LogError("No applicant found for decision evaluation.");
            return;
        }

        Validator validator = new Validator();
        List<OriginSymbolPair> shuffledPairs = applicantManager.GetShuffledPairs();

        if (shuffledPairs == null || shuffledPairs.Count == 0)
        {
            Debug.LogError("Shuffled pairs list is null or empty.");
            return;
        }

        bool isBirthYearValid = validator.IsValidBirthYear(applicant.BirthYear, CurrentYear);
        bool isExpirationValid = validator.IsValidExpirationYear(applicant.ExpirationYear, CurrentYear);
        bool isOriginValid = validator.IsValidOriginSymbol(applicant.Origin, applicant.RegionSymbol, shuffledPairs);

        if (isApproved)
        {
            if (isBirthYearValid && isExpirationValid && isOriginValid)
            {
                uiManager.UpdateResultText(resultText, "Correct Decision! Applicant Approved.");
            }
            else
            {
                gameController.AddFine(); // Add fine for invalid approval
                AdjustReputation(-reputationPenalty);
                uiManager.UpdateResultText(resultText, "Invalid applicant approved! Fine incurred.");
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
                AdjustReputation(-reputationPenalty); // Adjust reputation for valid denial
                uiManager.UpdateResultText(resultText, "Valid applicant denied! Reputation penalty applied.");
            }
        }

        Invoke(nameof(InvokeLoadNextApplicant), DecisionDelay);
    }


    private void AdjustReputation(int amount)
    {
        Debug.Log($"AdjustReputation called. Current Reputation: {currentReputation}, Adjustment: {amount}");

        currentReputation += amount;
        currentReputation = Mathf.Clamp(currentReputation, 0, maxReputation);

        uiManager.UpdateReputationBar(reputationBar, currentReputation, maxReputation);

        if (currentReputation <= 0)
        {
            Debug.Log("Reputation dropped to zero. The player can still process applicants until the end of the day.");
        }
    }


    private void GameOver(string message)
    {
        isGameOver = true;
        uiManager.DisableAllButtons();
        uiManager.HandleGameOver(gameOverPanel, resultText, message);
        Debug.Log(message);
    }

    private void InvokeLoadNextApplicant()
    {
        if (applicantManager.AreAllApplicantsProcessed())
        {
            // Show end-of-day summary
            int endOfDayEarnings = gameController.CalculateEndOfDayEarnings(currentReputation > 0);
            gameController.AddCoins(endOfDayEarnings);

            if (currentReputation > 0)
            {
                gameController.ShowEndOfDayMessage(false); // Successful day
            }
            else
            {
                gameController. ShowEndOfDayMessage(true); // Unsuccessful day with summary
            }

            return;
        }

        if (applicantManager.GenerateNextApplicant())
        {
            Applicant nextApplicant = applicantManager.GetCurrentApplicant();
            if (nextApplicant != null)
            {
                uiApplicantPanelManager.CloseDocument(); // Reset for the next applicant
                uiApplicantPanelManager.UpdateApplicantUI(nextApplicant);
                uiManager.UpdateResultText(resultText, "Awaiting decision...");
            }
        }
        else
        {
            Debug.Log("No more applicants to load.");
        }
    }




}
