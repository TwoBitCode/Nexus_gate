using UnityEngine;
using TMPro;

public class DecisionManager : MonoBehaviour
{
    public DocumentManager docManager; // Reference to DocumentManager
    public TextMeshProUGUI resultText; // Feedback text
    public TextMeshProUGUI endGameText; // Display end-game message

    private int humanReputation = 50; // Human faction reputation
    private int alienReputation = 50; // Alien faction reputation
    private int applicantsProcessed = 0; // Tracks processed applicants
    private int maxApplicants = 20; // Endgame after max applicants
    private int level = 1; // Difficulty level

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
        // Parse the document details
        string[] documentLines = docManager.documentText.text.Split('\n');
        string origin = documentLines[2].Split(':')[1].Trim();
        string documentType = documentLines[3].Split(':')[1].Trim();
        bool isAlien = origin == "Zarquinia" || origin == "Nebulon IV" || origin == "Andromeda Prime";

        if (isAlien)
        {
            // Handle Alien Cases
            if (isApproved)
            {
                alienReputation += 10; // Positive impact for aliens
                resultText.text = "Alien Accepted!";
            }
            else
            {
                alienReputation -= 10; // Negative impact for aliens
                resultText.text = "Alien Denied!";
            }
        }
        else
        {
            // Handle Human Cases
            int age = int.Parse(documentLines[1].Split(':')[1].Trim());

            if (age < 16 || age > 65)
            {
                // Human Age Invalid
                if (isApproved)
                {
                    humanReputation -= 5; // Penalize for approving an invalid human
                    resultText.text = "Error: Human Age Invalid!";
                }
                else
                {
                    resultText.text = "Human Denied (Correct)!";
                }
            }
            else
            {
                // Human Age is Valid
                if (isApproved)
                {
                    humanReputation += 5; // Reward for approving a valid human
                    resultText.text = "Human Accepted!";
                }
                else
                {
                    humanReputation -= 5; // Penalize for denying a valid human
                    resultText.text = "Error: Valid Human Denied!";
                }
            }
        }

        // Check reputations for potential end-game triggers
        CheckReputationLevels();

        // Move to next applicant after delay
        Invoke(nameof(NextApplicant), 1.5f);
    }

    private void CheckReputationLevels()
    {
        if (humanReputation <= 0)
        {
            EndGame("Humans have lost all trust in you!");
        }
        else if (alienReputation <= 0)
        {
            EndGame("Aliens are furious and refuse to cooperate!");
        }
    }

    private void NextApplicant()
    {
        applicantsProcessed++;

        // Introduce dynamic difficulty
        if (applicantsProcessed % 5 == 0)
        {
            level++;
            Debug.Log($"Level Up! Now at Level {level}");
        }

        if (applicantsProcessed >= maxApplicants)
        {
            EndGame("You have processed all applicants!");
            return;
        }

        // Reset the result text and generate a new document
        resultText.text = "";
        docManager.GenerateDocument();

        // Trigger random events occasionally
        if (Random.Range(0, 100) < 20) // 20% chance
        {
            TriggerRandomEvent();
        }
    }

    private void EndGame(string message)
    {
        // Display the end-game message
        endGameText.text = message;
        endGameText.gameObject.SetActive(true); // Ensure EndGameText is visible

        // Hide ResultText to avoid overlap
        resultText.text = "";
        resultText.gameObject.SetActive(false);

        // Disable buttons
        GameObject.Find("ApproveButton").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.Find("DenyButton").GetComponent<UnityEngine.UI.Button>().interactable = false;

        // Stop further applicant processing
        CancelInvoke(nameof(NextApplicant));
    }



    private void TriggerRandomEvent()
    {
        int eventChance = Random.Range(0, 100);

        if (eventChance < 10)
        {
            resultText.text = "Security Alert! Immediate decision required!";
        }
        else if (eventChance < 20)
        {
            resultText.text = "VIP Alien requests urgent clearance!";
        }
        else
        {
            resultText.text = "Anomaly detected in document verification!";
        }
    }
}
