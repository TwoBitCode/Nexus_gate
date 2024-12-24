using UnityEngine;

public class PassportManager : MonoBehaviour
{
    public ApplicantManager applicantManager; // Handles applicant data
    public UIApplicantPanelManager uiPanelManager; // Handles UI updates
    public GameController gameController; // For end-of-day transitions

    private DayData currentDayData;

    public void SetDayData(DayData dayData)
    {
        if (dayData == null)
        {
            Debug.LogError("SetDayData received a null DayData!");
            return;
        }

        currentDayData = dayData;
        applicantManager.SetDayData(dayData); // Delegate to ApplicantManager
    }

    public void GenerateNextApplicant()
    {
        Applicant applicant = applicantManager.GenerateApplicant();
        if (applicant == null)
        {
            EndDay();
            return;
        }

        uiPanelManager.UpdateApplicantUI(applicant);
    }


    public void OpenDocument()
    {
        // Get the current applicant from ApplicantManager
        Applicant currentApplicant = applicantManager.GetCurrentApplicant();
        if (currentApplicant == null)
        {
            Debug.LogError("No current applicant to display!");
            return;
        }

        // Open the document via UIApplicantPanelManager
        uiPanelManager.OpenDocument(currentApplicant);
    }

    public void CloseDocument()
    {
        // Close the document via UIApplicantPanelManager
        uiPanelManager.CloseDocument();
    }

    private void EndDay()
    {
        Debug.Log("End of Day! All applicants have been processed.");
        uiPanelManager.ToggleDocumentPanel(false); // Hide UI panels

        if (gameController != null)
        {
            gameController.ShowEndOfDayMessage(false); // Show end-of-day message
        }
        else
        {
            Debug.LogError("GameController is not assigned in PassportManager!");
        }
    }
}
