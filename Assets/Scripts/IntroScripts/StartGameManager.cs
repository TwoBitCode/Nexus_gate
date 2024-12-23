using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;
    public GameObject rulesPanel;
    public GameObject applicantPanel;
    public GameObject documentPanel;

    [Header("Game Managers")]
    public PassportManager passportManager;

    private void Start()
    {
        // Validate UI elements and ensure PassportManager reference exists
        if (!ValidateUIElements()) return;

        // Initial state: Show rules, hide applicant and document panels
        rulesPanel.SetActive(true);
        applicantPanel.SetActive(false);
        documentPanel.SetActive(false);

        // Attach the Start button listener
        if (startButton != null)
        {
            startButton.onClick.AddListener(() => StartCoroutine(StartGameRoutine()));
        }
        else
        {
            Debug.LogError("Start button not assigned!");
        }
    }

    private IEnumerator StartGameRoutine()
    {
        Debug.Log("Start button clicked!");

        // Check if PassportManager exists
        if (passportManager == null)
        {
            Debug.LogError("PassportManager reference is missing!");
            yield break;
        }

        // Load PassportManager resources
        passportManager.LoadResources();
        Debug.Log("Loading resources...");

        // Wait for resources to load if a check is needed
        while (!passportManager.AreResourcesLoaded())
        {
            Debug.Log("Waiting for resources to load...");
            yield return null; // Wait for the next frame
        }

        Debug.Log("Resources loaded. Starting game...");

        // Update UI state: Hide rules, show applicant panel
        rulesPanel.SetActive(false);
        applicantPanel.SetActive(true);

        // Generate the first passport
        passportManager.GeneratePassport();
        Debug.Log("Game Started: Passport generated successfully.");
    }

    private bool ValidateUIElements()
    {
        bool isValid = true;

        if (rulesPanel == null)
        {
            Debug.LogError("Rules panel is not assigned!");
            isValid = false;
        }

        if (applicantPanel == null)
        {
            Debug.LogError("Applicant panel is not assigned!");
            isValid = false;
        }

        if (documentPanel == null)
        {
            Debug.LogError("Document panel is not assigned!");
            isValid = false;
        }

        if (passportManager == null)
        {
            Debug.LogError("PassportManager is not assigned!");
            isValid = false;
        }

        return isValid;
    }
}
