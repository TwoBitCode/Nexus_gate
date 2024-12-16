using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DecisionManager : MonoBehaviour
{
    [Header("References")]
    public PassportManager passportManager;
    public TextMeshProUGUI resultText;
    public GameObject gameOverPanel;

    [Header("Reputation Settings")]
    [SerializeField] private int maxReputation = 100;
    [SerializeField] private int reputationPenalty = 10;
    [SerializeField] private int dangerousEntryPenalty = 100;

    private int currentReputation;
    private bool isGameOver = false;

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
        int age = int.Parse(SafeExtractField(passportData, 1, "Age"));

        bool isValid = IsValidApplicant(origin, age);
        bool isDangerous = IsDangerousApplicant(origin);

        if (isApproved)
        {
            if (isDangerous)
            {
                GameOver($"You approved a dangerous applicant from {origin}. They pose a threat to the station!");
                return;
            }
            else if (!isValid)
            {
                GameOver($"You approved an invalid applicant! Origin: {origin}, Age: {age}. Check the rules carefully next time.");
                return;
            }
            else
            {
                resultText.text = "Correct Decision! Applicant Approved.";
            }
        }
        else // Denied
        {
            if (isValid)
            {
                AdjustReputation(-reputationPenalty);
                resultText.text = $"Wrong Decision! Authorized applicant from {origin} denied. Reputation decreased.";
            }
            else
            {
                resultText.text = "Correct Decision! Unauthorized Applicant Denied.";
            }
        }

        Invoke(nameof(GenerateNextApplicant), 2.0f);
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
        catch { Debug.LogError($"Failed to extract '{fieldName}' from passport."); return string.Empty; }
    }

    private bool IsValidApplicant(string origin, int age)
    {
        string[] validOrigins = { "Zarquinia", "Nebulon IV", "Andromeda Prime", "Galva-Theta", "Xyron-9" };
        return System.Array.Exists(validOrigins, o => o == origin) && age >= passportManager.minAlienAge && age <= passportManager.maxAlienAge;
    }

    private bool IsDangerousApplicant(string origin)
    {
        string[] dangerousOrigins = { "Phantom Ring", "Oblivion Expanse" };
        return System.Array.Exists(dangerousOrigins, o => o == origin);
    }
}
