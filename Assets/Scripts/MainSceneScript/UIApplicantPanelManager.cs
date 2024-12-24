using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class UIApplicantPanelManager : MonoBehaviour
{
    public Image applicantImage;
    public Image regionSymbol;
    public TextMeshProUGUI passportText;
    public TextMeshProUGUI dialogueText;

    public GameObject applicantPanel;
    public GameObject documentPanel;

    public Button approveButton;
    public Button denyButton;
    private bool isPassportOpened = false;

    public bool IsPassportOpened => isPassportOpened;

    public void UpdateApplicantUI(Applicant applicant)
    {
        if (applicantImage == null || regionSymbol == null || passportText == null)
        {
            Debug.LogError("UI elements are not assigned!");
            return;
        }

        applicantImage.sprite = applicant.FaceImage;
        regionSymbol.sprite = applicant.RegionSymbol;
        passportText.text = $"Name: {applicant.Name}\n" +
                            $"Date of Birth: {applicant.BirthYear}\n" +
                            $"Origin: {applicant.Origin}\n" +
                            $"Expiration Year: {applicant.ExpirationYear}";

        applicantPanel.SetActive(true);
    }

    public void ToggleDocumentPanel(bool isVisible)
    {
        if (documentPanel != null)
        {
            documentPanel.SetActive(isVisible);
        }
    }

    public void EnableDecisionButtons(bool enable)
    {
        if (approveButton != null) approveButton.interactable = enable;
        if (denyButton != null) denyButton.interactable = enable;
    }
    public void ShowRandomDialogue(Applicant applicant)
    {
        if (applicant.RandomDialogues == null || applicant.RandomDialogues.Length == 0)
        {
            Debug.LogError("No dialogues available for the applicant!");
            dialogueText.text = "No dialogue available."; // Fallback message
            return;
        }

        string randomDialogue = applicant.RandomDialogues[Random.Range(0, applicant.RandomDialogues.Length)];
        dialogueText.text = randomDialogue;
        Debug.Log($"Displayed random dialogue: {randomDialogue}");
    }

    public void OpenDocument(Applicant applicant)
    {
        isPassportOpened = true; // Mark that the passport was opened

        if (documentPanel != null)
        {
            documentPanel.SetActive(true); // Show the document panel
        }

        if (applicant != null)
        {
            passportText.text = $"Name: {applicant.Name}\n" +
                                $"Date of Birth: {applicant.BirthYear}\n" +
                                $"Origin: {applicant.Origin}\n" +
                                $"Expiration Year: {applicant.ExpirationYear}";
            ShowRandomDialogue(applicant);
        }

        EnableDecisionButtons(false); // Disable decision buttons when viewing the document
    }


    public void CloseDocument()
    {
        if (documentPanel != null)
        {
            documentPanel.SetActive(false); // Hide the document panel
        }

        EnableDecisionButtons(true); // Re-enable decision buttons
    }

}
