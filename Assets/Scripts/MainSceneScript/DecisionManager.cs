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
                AdjustReputation(-reputationPenalty);
                GameOver("You approved an invalid applicant! Critical Error.");
                return;
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
                AdjustReputation(-reputationPenalty);
                uiManager.UpdateResultText(resultText, "Wrong Decision! Valid applicant denied.");
            }
        }

        if (!isGameOver)
        {
            Invoke(nameof(InvokeLoadNextApplicant), DecisionDelay);
        }
    }

    private void AdjustReputation(int amount)
    {
        Debug.Log($"AdjustReputation called. Current Reputation: {currentReputation}, Adjustment: {amount}");

        currentReputation += amount;
        currentReputation = Mathf.Clamp(currentReputation, 0, maxReputation);

        uiManager.UpdateReputationBar(reputationBar, currentReputation, maxReputation);

        if (currentReputation <= 0)
        {
            GameOver("Your reputation has dropped to zero! Game Over.");
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
            gameController.ShowEndOfDayMessage(currentReputation <= 0);
            return;
        }

        if (applicantManager.GenerateNextApplicant())
        {
            Applicant nextApplicant = applicantManager.GetCurrentApplicant();
            if (nextApplicant != null)
            {
                uiApplicantPanelManager.UpdateApplicantUI(nextApplicant);
                uiManager.UpdateResultText(resultText, "");
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
