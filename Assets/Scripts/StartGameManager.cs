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
        if (!ValidateUIElements()) return;

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

        if (passportManager == null)
        {
            Debug.LogError("PassportManager reference is missing!");
            yield break;
        }

        passportManager.LoadResources();

        while (!passportManager.IsResourcesLoaded())
        {
            Debug.Log("Waiting for resources to load...");
            yield return null; // Wait for one frame
        }

        Debug.Log("Resources loaded. Starting game...");

        rulesPanel.SetActive(false);
        applicantPanel.SetActive(true);

        passportManager.GeneratePassport();
        Debug.Log("Game Started: Passport generated successfully.");
    }

    private bool ValidateUIElements()
    {
        if (rulesPanel == null || applicantPanel == null || documentPanel == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the inspector!");
            return false;
        }

        if (passportManager == null)
        {
            Debug.LogError("PassportManager is not assigned!");
            return false;
        }

        return true;
    }
}
