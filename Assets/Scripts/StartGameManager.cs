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
        rulesPanel.SetActive(true);
        applicantPanel.SetActive(false);
        documentPanel.SetActive(false);

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

        // Trigger resource loading explicitly
        passportManager.LoadResources();

        // Wait until resources are loaded
        while (!passportManager.IsResourcesLoaded())
        {
            Debug.Log("Waiting for resources to load...");
            yield return null; // Wait one frame
        }

        Debug.Log("Resources loaded. Starting game...");

        // Hide the Rules Panel and show Applicant Panel
        rulesPanel.SetActive(false);
        applicantPanel.SetActive(true);

        passportManager.GeneratePassport();
        Debug.Log("Game Started: Passport generated successfully.");
    }

}
