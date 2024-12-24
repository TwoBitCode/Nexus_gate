using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class StartGameManager : MonoBehaviour
{
    [Header("Day Tracking")]
    public int currentDay = 1;
    public DayData[] allDayData;

    [Header("UI Elements")]
    public Button startButton;
    public GameObject rulesPanel;
    public TextMeshProUGUI rulesText;
    public GameObject applicantPanel;
    public GameObject documentPanel;
    public Slider reputationBar; // Reference to the reputation bar
    public int maxReputation = 100; // Maximum reputation value


    [Header("Game Managers")]
    public PassportManager passportManager;

    private DayData currentDayData;

    private void Start()
    {
        LoadDayData();
        InitializeDay();
    }

    private void LoadDayData()
    {
        if (currentDay - 1 < allDayData.Length)
        {
            currentDayData = allDayData[currentDay - 1];
        }
        else
        {
            Debug.LogError("DayData for the current day is missing!");
        }
    }

    private void InitializeDay()
    {
        if (!ValidateUIElements()) return;

        // Display rules panel
        rulesPanel.SetActive(true);
        applicantPanel.SetActive(false);
        documentPanel.SetActive(false);

        // Set rules text
        if (currentDayData != null && rulesText != null)
        {
            rulesText.text = string.Join("\n", currentDayData.newRules);
        }
        else
        {
            Debug.LogError("DayData or Rules Text is missing!");
        }

        // Initialize reputation bar
        if (reputationBar != null)
        {
            reputationBar.maxValue = maxReputation;
            reputationBar.value = maxReputation; // Fill the bar to the maximum
            Debug.Log($"ReputationBar initialized: maxValue={maxReputation}, value={maxReputation}");
        }
        else
        {
            Debug.LogError("ReputationBar is not assigned in the Inspector!");
        }

        // Attach listener to start button
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartGame);
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        Debug.Log("Start button clicked!");

        // Validate PassportManager
        if (passportManager == null)
        {
            Debug.LogError("PassportManager reference is missing!");
            yield break;
        }

        // Validate DayData
        if (currentDayData == null)
        {
            Debug.LogError("DayData is missing for the current day!");
            yield break;
        }

        // Pass the DayData to PassportManager
        passportManager.SetDayData(currentDayData);

        // Transition to applicant panel
        rulesPanel.SetActive(false);
        applicantPanel.SetActive(true);

        // Generate the first passport
        passportManager.GeneratePassport();
        Debug.Log("Game Started: Passport generated successfully.");

        yield break;
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

        if (rulesText == null)
        {
            Debug.LogError("Rules text (TextMeshPro) is not assigned!");
            isValid = false;
        }

        if (currentDayData == null)
        {
            Debug.LogError("DayData is not assigned!");
            isValid = false;
        }

        return isValid;
    }
}
