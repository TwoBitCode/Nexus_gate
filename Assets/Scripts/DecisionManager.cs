using UnityEngine;
using TMPro;

public class DecisionManager : MonoBehaviour
{
    [Header("References")]
    public DocumentManager docManager; // Reference to DocumentManager
    public TextMeshProUGUI resultText; // Feedback text
    public TextMeshProUGUI endGameText; // Display end-game message
    public GameObject gameOverPanel; // Reference to the Game Over panel

    [Header("Reputation Settings")]
    [SerializeField] private int startingReputation = 50; // Starting reputation for both factions
    [SerializeField] private int reputationGain = 10; // Reputation increase for correct approvals
    [SerializeField] private int reputationLoss = 5; // Reputation loss for incorrect approvals
    [SerializeField] private int unknownOriginPenalty = 10; // Penalty for approving unknown origins

    [Header("Applicant Settings")]
    [SerializeField] private int minAge = 16; // Minimum valid age
    [SerializeField] private int maxAge = 65; // Maximum valid age
    [SerializeField] private int maxApplicants = 10; // Maximum number of applicants to process

    [Header("Timing Settings")]
    [SerializeField] private float nextApplicantDelay = 1.5f; // Delay before moving to the next applicant

    private int humanReputation;
    private int alienReputation;
    private int applicantsProcessed = 0; // Tracks processed applicants
    private bool isGameOver = false; // Flag to prevent further actions after game over

    private void Start()
    {
        // Initialize reputation values
        humanReputation = startingReputation;
        alienReputation = startingReputation;
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
        // Ensure document lines are available and well-formed
        string[] documentLines = docManager.documentText.text.Split('\n');
        if (documentLines.Length < 4)
        {
            Debug.LogError("Document data is incomplete or malformed.");
            EndGameImmediately("Error: Document data corrupted.");
            return;
        }

        // Extract and trim details from document
        string origin = SafeExtractField(documentLines, 2, "Origin");
        string documentType = SafeExtractField(documentLines, 3, "Document");

        Debug.Log($"Parsed Origin: '{origin}'");

        // Define alien and human planets
        string[] alienPlanets = { "Zarquinia", "Nebulon IV", "Andromeda Prime" };
        string[] humanPlanets = { "Earth", "Mars", "Jupiter" };

        // Check applicant type
        bool isAlien = System.Array.Exists(alienPlanets, planet => planet.Equals(origin, System.StringComparison.OrdinalIgnoreCase));
        bool isHuman = System.Array.Exists(humanPlanets, planet => planet.Equals(origin, System.StringComparison.OrdinalIgnoreCase));

        if (isAlien)
        {
            ProcessAlien(isApproved, origin);
        }
        else if (isHuman)
        {
            ProcessHuman(isApproved, origin, documentLines);
        }
        else
        {
            HandleUnknownOrigin(isApproved, origin);
        }

        // Move to the next applicant after a delay
        Invoke(nameof(NextApplicant), nextApplicantDelay);
    }

    private string SafeExtractField(string[] lines, int index, string fieldName)
    {
        try
        {
            return lines[index].Split(':')[1].Trim();
        }
        catch
        {
            Debug.LogError($"Failed to extract '{fieldName}' from document.");
            return string.Empty;
        }
    }

    private void ProcessAlien(bool isApproved, string origin)
    {
        if (isApproved)
        {
            alienReputation += reputationGain; // Positive impact for aliens
            resultText.text = $"Alien from {origin} Approved!";
        }
        else
        {
            alienReputation -= reputationGain; // Negative impact for aliens
            resultText.text = $"Alien from {origin} Denied!";
        }
    }

    private void ProcessHuman(bool isApproved, string origin, string[] documentLines)
    {
        int age;
        if (!int.TryParse(SafeExtractField(documentLines, 1, "Age"), out age))
        {
            Debug.LogError("Failed to parse age from document.");
            EndGameImmediately($"Error: Invalid age format for Human from {origin}.");
            return;
        }

        if (age < minAge || age > maxAge)
        {
            if (isApproved)
            {
                humanReputation -= reputationLoss;
                EndGameImmediately($"Error: Human from {origin} - Age Invalid!");
            }
            else
            {
                resultText.text = $"Human from {origin} Denied (Correct)!";
            }
        }
        else
        {
            if (isApproved)
            {
                humanReputation += reputationGain; // Reward for approving a valid human
                resultText.text = $"Human from {origin} Approved!";
            }
            else
            {
                humanReputation -= reputationLoss;
                EndGameImmediately($"Error: Valid Human from {origin} Denied!");
            }
        }
    }

    private void HandleUnknownOrigin(bool isApproved, string origin)
    {
        if (isApproved)
        {
            humanReputation -= unknownOriginPenalty;
            EndGameImmediately($"Critical System Failure: Unknown Origin '{origin}' Approved!");
        }
        else
        {
            resultText.text = $"Applicant from Unknown Origin '{origin}' Denied (Correct)!";
        }
    }

    private void NextApplicant()
    {
        if (isGameOver) return;

        applicantsProcessed++;

        if (applicantsProcessed >= maxApplicants)
        {
            EndGameImmediately("You have processed all applicants!");
            return;
        }

        resultText.text = "Waiting for Decision...";
        docManager.GenerateDocument();
    }

    private void EndGameImmediately(string message)
    {
        if (isGameOver) return;

        isGameOver = true; // Prevent further actions

        CancelInvoke(nameof(NextApplicant)); // Stop delayed processing

        docManager.documentText.text = "";
        resultText.text = "";

        endGameText.text = message;
        gameOverPanel.SetActive(true);

        GameObject.Find("ApproveButton").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.Find("DenyButton").GetComponent<UnityEngine.UI.Button>().interactable = false;
    }
}
