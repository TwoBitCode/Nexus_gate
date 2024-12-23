using UnityEngine;
using TMPro;

public class DecisionManager : MonoBehaviour
{
    [Header("References")]
    public PassportManager passportManager;
    public TextMeshProUGUI resultText;
    public GameObject gameOverPanel;

    [Header("Reputation Settings")]
    [SerializeField] private int maxReputation = 100;
    [SerializeField] private int reputationPenalty = 10;

    [Header("Decision Settings")]
    [SerializeField] private float nextApplicantDelay = 2.0f; // Delay before showing next applicant

    [Header("Applicant Settings")]
    [SerializeField] private string[] validOrigins = { "Zarquinia", "Nebulon IV", "Andromeda Prime", "Galva-Theta", "Xyron-9" };
    [SerializeField] private string[] dangerousOrigins = { "Phantom Ring", "Oblivion Expanse" };

    private int currentReputation;
    private bool isGameOver = false;
    private int currentYear = 4065; // Current game year

    private void Start()
    {
        currentReputation = maxReputation;
        resultText.text = ""; // Initialize feedback text
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
        string[] passportData = passportManager.passportText.text.Split('\n');

        string origin = SafeExtractField(passportData, 2, "Origin");
        int birthYear = SafeParseYear(SafeExtractField(passportData, 1, "Date of Birth"));
        int expirationYear = SafeParseYear(SafeExtractField(passportData, 3, "Expiration Year"));

        if (birthYear < 0 || expirationYear < 0) return; // Invalid data check

        bool isExpired = expirationYear < currentYear;
        bool isFakeOrigin = !System.Array.Exists(validOrigins, o => o == origin);
        bool isDangerous = System.Array.Exists(dangerousOrigins, o => o == origin);

        if (isApproved)
        {
            if (isDangerous)
            {
                GameOver($"You approved a dangerous applicant from {origin}. They pose a threat to the station!");
                return;
            }
            else if (isExpired || isFakeOrigin)
            {
                string reason = isExpired ? "expired passport" : "fake origin";
                GameOver($"You approved an invalid applicant! Reason: {reason}. Check the passport carefully next time.");
                return;
            }
            else
            {
                resultText.text = "Correct Decision! Applicant Approved.";
            }
        }
        else // Denied
        {
            if (!isExpired && !isFakeOrigin && !isDangerous)
            {
                AdjustReputation(-reputationPenalty);
                resultText.text = $"Wrong Decision! Valid applicant from {origin} denied. Reputation decreased.";
            }
            else
            {
                resultText.text = "Correct Decision! Unauthorized Applicant Denied.";
            }
        }

        Invoke(nameof(GenerateNextApplicant), nextApplicantDelay);
    }

    private void AdjustReputation(int amount)
    {
        currentReputation += amount;
        if (currentReputation <= 0)
        {
            GameOver("Your reputation has dropped to zero! Game Over.");
        }
    }

    private void GenerateNextApplicant()
    {
        if (isGameOver) return;

        resultText.text = ""; // Reset feedback text
        passportManager.GeneratePassport();
    }

    private void GameOver(string message)
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over Triggered.");
        resultText.text = message;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Game Over panel is not assigned in the inspector!");
        }
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
