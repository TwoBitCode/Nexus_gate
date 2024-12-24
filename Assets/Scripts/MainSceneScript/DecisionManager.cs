using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class DecisionManager : MonoBehaviour
{
    [Header("References")]
    public PassportManager passportManager;
    public TextMeshProUGUI resultText;
    public GameObject gameOverPanel;

    [Header("Day Data")]
    public DayData currentDayData; // Dynamically load the DayData for the current day

    [Header("Reputation Settings")]
    public Slider reputationBar; // Reference to the UI slider
    [SerializeField] private int maxReputation = 100;
    [SerializeField] private int reputationPenalty = 10;

    private int currentReputation;
    private bool isGameOver = false;
    //private int currentYear = 4065; // Current game year



    private void Start()
    {
        currentReputation = maxReputation;
        if (reputationBar != null)
        {
            reputationBar.maxValue = maxReputation; // Set the maximum value for the bar
            reputationBar.value = maxReputation;    // Fill the bar to the maximum at the start
        }
        else
        {
            Debug.LogError("ReputationBar is not assigned in the Inspector!");
        }

        resultText.text = ""; // Clear the result text at the start
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
        // Extract passport data
        string[] passportData = passportManager.passportText.text.Split('\n');
        string origin = SafeExtractField(passportData, 2, "Origin");
        int birthYear = SafeParseYear(SafeExtractField(passportData, 1, "Date of Birth"));
        int expirationYear = SafeParseYear(SafeExtractField(passportData, 3, "Expiration Year"));
        Sprite displayedSymbol = passportManager.regionSymbol.sprite;

        bool isOriginValid = passportManager.IsValidOriginSymbol(origin, displayedSymbol);
        bool isBirthYearValid = birthYear <= 4065 && birthYear >= 4015;
        bool isExpirationValid = expirationYear >= 4065;

        if (isApproved)
        {
            if (isOriginValid && isBirthYearValid && isExpirationValid)
            {
                resultText.text = "Correct Decision! Applicant Approved.";
            }
            else
            {
                string reason = !isOriginValid
                    ? "Origin and symbol mismatch"
                    : (!isBirthYearValid ? "Invalid birth year" : "Expired passport");
                GameOver($"You approved an invalid applicant! Reason: {reason}");
                return;
            }
        }
        else // Denied
        {
            if (!isOriginValid || !isBirthYearValid || !isExpirationValid)
            {
                resultText.text = "Correct Decision! Unauthorized Applicant Denied.";
            }
            else
            {
                resultText.text = "Wrong Decision! Valid applicant denied.";
                AdjustReputation(-reputationPenalty); // Deduct reputation
            }
        }

        Invoke(nameof(LoadNextApplicant), 1.5f);
    }




    private void AdjustReputation(int amount)
    {
        currentReputation += amount;
        reputationBar.value = currentReputation; // Update the UI slider

        if (currentReputation <= 0)
        {
            GameOver("Your reputation has dropped to zero! Game Over.");
        }
    }


    private void LoadNextApplicant()
    {
        Debug.Log("LoadNextApplicant called.");

        if (!passportManager.GenerateNextPassport())
        {
            Debug.Log("End of day reached.");
            resultText.text = "No more applicants for today!";
            return;
        }

        // Clear result text and prepare for the next decision
        resultText.text = "Awaiting decision...";
        Debug.Log("Ready for next applicant.");
    }

    [Header("Game Controller")]
    public GameController gameController;

    private void GameOver(string message)
    {
        isGameOver = true;

        if (gameController != null)
        {
            gameController.ShowEndOfDayMessage(true); // Game over message
        }
        else
        {
            Debug.LogError("GameController is not assigned in DecisionManager!");
        }

        Debug.Log(message);
    }




    private string SafeExtractField(string[] lines, int index, string fieldName)
    {
        try { return lines[index].Split(':')[1].Trim(); }
        catch
        {
            Debug.LogError($"Failed to extract '{fieldName}' from passport.");
            return string.Empty;
        }
    }

    private int SafeParseYear(string yearString)
    {
        if (int.TryParse(yearString, out int year))
        {
            return year;
        }
        Debug.LogError($"Failed to parse year: {yearString}. Ensure the passport data format is correct.");
        return -1; // Invalid year


    }
}
