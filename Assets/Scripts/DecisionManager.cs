using UnityEngine;
using TMPro;

public class DecisionManager : MonoBehaviour
{
    public DocumentManager docManager;
    public TextMeshProUGUI resultText;

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
        // Ensure the document text is not empty
        if (string.IsNullOrEmpty(docManager.documentText.text))
        {
            resultText.text = "Error: Document not generated!";
            return;
        }

        // Parse document text
        string[] documentLines = docManager.documentText.text.Split('\n');

        if (documentLines.Length < 4)
        {
            resultText.text = "Error: Invalid document format!";
            return;
        }

        // Extract age value
        int age;
        if (!int.TryParse(documentLines[1].Split(':')[1].Trim(), out age))
        {
            resultText.text = "Error: Invalid age value!";
            return;
        }

        // Check decision based on the age field
        if (age < 18 && isApproved)
        {
            resultText.text = "Error: Underage Applicant Approved!";
        }
        else if (age >= 18 && !isApproved)
        {
            resultText.text = "Error: Eligible Applicant Denied!";
        }
        else
        {
            resultText.text = "Correct Decision!";
        }

        // Wait a moment and move to the next applicant
        Invoke(nameof(NextApplicant), 1.5f); // Delay for feedback
    }

    public int applicantsProcessed = 0; // Tracks the number of applicants

    private void NextApplicant()
    {
        applicantsProcessed++;
        resultText.text = ""; // Clear the result text
        docManager.GenerateDocument(); // Generate the next applicant's document

        // Update the UI counter
        TextMeshProUGUI counterText = GameObject.Find("ApplicantCounter").GetComponent<TextMeshProUGUI>();
        counterText.text = $"Applicants Processed: {applicantsProcessed}";
    }


}
