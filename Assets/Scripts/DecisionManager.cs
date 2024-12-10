using UnityEngine;
using TMPro;

public class DecisionManager : MonoBehaviour
{
    public DocumentManager docManager; // Reference to DocumentManager
    public TextMeshProUGUI resultText; // Feedback text
    public TextMeshProUGUI endGameText; // Display end-game message
    public GameObject gameOverPanel; // Reference to the Game Over panel

    private int humanReputation = 50; // Human faction reputation
    private int alienReputation = 50; // Alien faction reputation
    private int applicantsProcessed = 0; // Tracks processed applicants
    private int maxApplicants = 10; // Endgame after max applicants

    public void Approve()
    {
        EvaluateDecision(true);
    }

    public void Deny()
    {
        EvaluateDecision(false);
    }

    private void EvaluateDecision(bool isApproved)
    {
        // Split the document lines and trim spaces
        string[] documentLines = docManager.documentText.text.Split('\n');
        string origin = documentLines[2].Split(':')[1].Trim(); // Trim leading and trailing spaces
        string documentType = documentLines[3].Split(':')[1].Trim();

        Debug.Log($"Parsed Origin: '{origin}'");

        // Define alien and human planets
        string[] alienPlanets = { "Zarquinia", "Nebulon IV", "Andromeda Prime" };
        string[] humanPlanets = { "Earth", "Mars", "Jupiter" };

        // Ensure trimmed and normalized comparison
        bool isAlien = System.Array.Exists(alienPlanets, planet => planet.Equals(origin, System.StringComparison.OrdinalIgnoreCase));
        bool isHuman = System.Array.Exists(humanPlanets, planet => planet.Equals(origin, System.StringComparison.OrdinalIgnoreCase));

        if (isAlien)
        {
            Debug.Log($"Alien identified from {origin}");
            if (isApproved)
            {
                alienReputation += 10; // Positive impact for aliens
                resultText.text = $"Alien from {origin} Approved!";
            }
            else
            {
                alienReputation -= 10; // Negative impact for aliens
                resultText.text = $"Alien from {origin} Denied!";
            }
        }
        else if (isHuman)
        {
            Debug.Log($"Human identified from {origin}");
            int age = int.Parse(documentLines[1].Split(':')[1].Trim()); // Ensure age is also trimmed

            if (age < 16 || age > 65)
            {
                if (isApproved)
                {
                    humanReputation -= 5;
                    EndGameImmediately($"Error: Human from {origin} - Age Invalid!");
                    return;
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
                    humanReputation += 5; // Reward for approving a valid human
                    resultText.text = $"Human from {origin} Approved!";
                }
                else
                {
                    humanReputation -= 5;
                    EndGameImmediately($"Error: Valid Human from {origin} Denied!");
                    return;
                }
            }
        }
        else
        {
            Debug.Log($"Unknown origin: {origin}");
            if (isApproved)
            {
                // Mistake: Approved an unknown origin
                humanReputation -= 10;
                EndGameImmediately($"Critical System Failure: Unknown Origin '{origin}' Approved!");
                return;
            }
            else
            {
                resultText.text = $"Applicant from Unknown Origin '{origin}' Denied (Correct)!";
            }
        }
        Invoke(nameof(NextApplicant), 1.5f);
    }





    private void NextApplicant()
    {
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
        // Cancel any further actions
        CancelInvoke(nameof(NextApplicant));

        // Clear applicant text
        docManager.documentText.text = "";

        // Clear feedback text
        resultText.text = "";

        // Display Game Over message
        endGameText.text = message;
        gameOverPanel.SetActive(true); // Show the Game Over panel

        // Disable buttons
        GameObject.Find("ApproveButton").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.Find("DenyButton").GetComponent<UnityEngine.UI.Button>().interactable = false;
    }

}
